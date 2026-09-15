using MediatR;
using Microsoft.Extensions.Configuration;
using exam_system.Common.Enums;
using exam_system.Common.Services;
using exam_system.Features.Identity.RefreshToken.Commands;
using exam_system.Features.Identity.RefreshToken.Orchestrators;
using exam_system.Features.Identity.RefreshToken.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class RefreshTokenOrchestratorHandler : IRequestHandler<RefreshTokenOrchestrator, RequestResponse<RefreshTokenResponse>>
{
    private readonly IMediator _mediator;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public RefreshTokenOrchestratorHandler(
        IMediator mediator,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<RequestResponse<RefreshTokenResponse>> Handle(RefreshTokenOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Verify token was provided in the cookie
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return RequestResponse<RefreshTokenResponse>.Fail("Refresh token is required.", statusCode: 401);
        }

        // 2. Fetch the token entity along with its User and all associated tokens
        var tokenEntity = await _mediator.Send(new GetRefreshTokenQuery(request.RefreshToken), cancellationToken);
        if (tokenEntity == null)
        {
            return RequestResponse<RefreshTokenResponse>.Fail("Invalid refresh token.", statusCode: 401);
        }

        // 3. Compromise Detection: If the token was already used, a replay attack is occurring
        if (tokenEntity.IsUsed)
        {
            if (tokenEntity.User?.RefreshTokens != null)
            {
                foreach (var activeToken in tokenEntity.User.RefreshTokens.Where(t => !t.IsRevoked))
                {
                    activeToken.IsRevoked = true;
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return RequestResponse<RefreshTokenResponse>.Fail(
                "Token reuse detected. All active sessions have been revoked. Please log in again.",
                statusCode: 401);
        }

        // 4. Check if the token has been revoked
        if (tokenEntity.IsRevoked)
        {
            return RequestResponse<RefreshTokenResponse>.Fail("Refresh token has been revoked.", statusCode: 401);
        }

        // 5. Check if the token has expired
        if (DateTime.UtcNow > tokenEntity.ExpiresAt)
        {
            return RequestResponse<RefreshTokenResponse>.Fail("Refresh token has expired.", statusCode: 401);
        }

        // 6. Check user status
        var user = tokenEntity.User;
        if (user == null || user.AccountStatus != AccountStatus.Active || !user.EmailConfirmed)
        {
            return RequestResponse<RefreshTokenResponse>.Fail("User account is not active.", statusCode: 403);
        }

        // 7. Mark the current token as used
        tokenEntity.IsUsed = true;

        // 8. Generate new access token and rotated refresh token
        var newAccessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var newRefreshTokenString = _jwtService.GenerateRefreshToken();

        tokenEntity.ReplacedByToken = newRefreshTokenString;

        var expiryDaysStr = _configuration["JwtSettings:RefreshTokenExpiryDays"];
        var expiryDays = int.TryParse(expiryDaysStr, out var parsedDays) ? parsedDays : 7;
        var expiresAt = DateTime.UtcNow.AddDays(expiryDays);

        await _mediator.Send(new AddRefreshTokenCommand(user.Id, newRefreshTokenString, expiresAt), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenString
        };

        return RequestResponse<RefreshTokenResponse>.Ok(response, "Session renewed successfully");
    }
}
