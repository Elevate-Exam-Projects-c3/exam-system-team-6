using MediatR;
using exam_system.Common.Enums;
using exam_system.Common.Services;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Orchestrators;

public class RegisterStudentOrchestrator(
    IEmailService emailService,
    IPasswordHasher passwordHasher,
    IOtpService otpService,
    IMediator mediator)
{
    public async Task<RequestResponse<RegisterResponse>> RegisterStudentAsync(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 1. Check if Email already exists via dedicated Query
        var emailExists = await mediator.Send(new CheckEmailExistsQuery(normalizedEmail), cancellationToken);
        if (emailExists)
            return RequestResponse<RegisterResponse>.Fail("Email already registered", statusCode: 409);

        // 2. Build user aggregate (hashing, OTP generation, entity construction)
        var plainOtp = otpService.GenerateOtp();

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = UserRole.Student,
            AccountStatus = AccountStatus.Pending,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow,
            Student = new Student
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        user.EmailVerificationOtps.Add(new EmailVerificationOtp
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Email = normalizedEmail,
            OtpHash = otpService.HashOtp(plainOtp),
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            CreatedAt = DateTime.UtcNow,
            IsUsed = false,
            AttemptCount = 0
        });

        // 3. Persist via dedicated command
        await mediator.Send(new CreateUserCommand(user), cancellationToken);

        // 4. Send OTP via email
        await emailService.SendEmailAsync(
            user.Email,
            "Verify your email",
            $"<p>Welcome {user.FullName},</p><p>Your verification code is: <strong>{plainOtp}</strong>. It expires in 10 minutes.</p>"
        );

        var responseData = new RegisterResponse
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Message = "Registration successful. Please verify your email with the OTP sent."
        };

        return RequestResponse<RegisterResponse>.Created(responseData, "User registered successfully");
    }
}
