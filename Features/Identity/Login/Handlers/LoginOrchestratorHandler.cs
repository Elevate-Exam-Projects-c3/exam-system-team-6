using MediatR;
using exam_system.Common.Enums;
using exam_system.Common.Services;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Identity.Login.Orchestrators;
using exam_system.Features.Identity.Login.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Login.Handlers;

public class LoginOrchestratorHandler : IRequestHandler<LoginOrchestrator, RequestResponse<LoginResponse>>
{
    private readonly IMediator _mediator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public LoginOrchestratorHandler(
        IMediator mediator,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<LoginResponse>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Fetch user by email
        var user = await _mediator.Send(new GetUserByEmailQuery { Email = request.Email }, cancellationToken);
        if (user == null)
        {
            return RequestResponse<LoginResponse>.Fail("Invalid email or password.", statusCode: 401);
        }

        // 2. Check lockout
        if (user.LockoutEnd.HasValue)
        {
            if (user.LockoutEnd.Value > DateTime.UtcNow)
            {
                return RequestResponse<LoginResponse>.Fail(
                    $"Account is locked due to too many failed login attempts. Please try again after {user.LockoutEnd.Value:HH:mm} UTC.",
                    statusCode: 403);
            }

            // Lockout period has elapsed, reset lockout state
            user.LockoutEnd = null;
            user.FailedLoginAttempts = 0;
        }

        // 3. Check account status & email confirmation (explanatory error, not invalid credentials)
        if (user.AccountStatus == AccountStatus.Pending || !user.EmailConfirmed)
        {
            return RequestResponse<LoginResponse>.Fail(
                "Your account is pending verification. Please verify your email before logging in.",
                statusCode: 403);
        }

        if (user.AccountStatus == AccountStatus.Suspended)
        {
            return RequestResponse<LoginResponse>.Fail(
                "Your account has been suspended. Please contact support.",
                statusCode: 403);
        }

        if (user.AccountStatus != AccountStatus.Active)
        {
            return RequestResponse<LoginResponse>.Fail(
                "Your account is not active. Please contact support.",
                statusCode: 403);
        }

        // 4. Verify password
        var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= 5)
            {
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return RequestResponse<LoginResponse>.Fail(
                    "Account locked for 15 minutes due to 5 consecutive failed login attempts.",
                    statusCode: 403);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return RequestResponse<LoginResponse>.Fail("Invalid email or password.", statusCode: 401);
        }

        // 5. Successful login: reset failed attempts & lockout
        user.FailedLoginAttempts = 0;
        user.LockoutEnd = null;

        // 6. Generate access & refresh tokens
        var tokens = await _mediator.Send(new GenerateTokensCommand
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role.ToString()
        }, cancellationToken);

        // 7. Persist changes to database
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 8. Return response
        var response = new LoginResponse
        {
            AccessToken = tokens.AccessToken,
            Email = user.Email,
            Role = user.Role.ToString(),
            RefreshToken = tokens.RefreshToken
        };

        return RequestResponse<LoginResponse>.Ok(response, "Login successful");
    }
}
