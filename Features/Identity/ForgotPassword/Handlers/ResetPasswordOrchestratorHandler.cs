using MediatR;
using exam_system.Common.Services;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class ResetPasswordOrchestratorHandler(
    IMediator mediator,
    IPasswordHasher passwordHasher) : IRequestHandler<ResetPasswordOrchestrator, RequestResponse>
{
    public async Task<RequestResponse> Handle(ResetPasswordOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Locate active PasswordResetOtp record via dedicated query projection
        var otpRecord = await mediator.Send(new GetResetOtpByTokenQuery(request.ResetToken), cancellationToken);

        if (otpRecord == null)
        {
            return RequestResponse.Fail(
                "Invalid or already used reset token. Please request a new verification code.",
                statusCode: 400);
        }

        // 2. Validate expiration
        if (!otpRecord.ResetTokenExpiresAt.HasValue || DateTime.UtcNow > otpRecord.ResetTokenExpiresAt.Value)
        {
            return RequestResponse.Fail(
                "Reset token has expired. Please request a new password reset.",
                statusCode: 400);
        }

        // 3. Verify user id
        if (otpRecord.UserId == Guid.Empty)
        {
            return RequestResponse.Fail("User not found.", statusCode: 400);
        }

        // 4. Hash the new password (BCrypt workFactor >= 12)
        var newPasswordHash = passwordHasher.Hash(request.NewPassword);

        // 5. Command 1: Update user password & clear lockout
        await mediator.Send(new UpdateUserPasswordCommand(otpRecord.UserId, newPasswordHash), cancellationToken);

        // 6. Command 2: Invalidate the reset token immediately
        await mediator.Send(new InvalidateResetTokenCommand(otpRecord.OtpId), cancellationToken);

        // 7. Command 3: Revoke all user refresh tokens (forcing re-login on all devices)
        await mediator.Send(new RevokeUserRefreshTokensCommand(otpRecord.UserId), cancellationToken);

        return RequestResponse.Ok("Password reset successfully. All active sessions have been signed out.");
    }
}
