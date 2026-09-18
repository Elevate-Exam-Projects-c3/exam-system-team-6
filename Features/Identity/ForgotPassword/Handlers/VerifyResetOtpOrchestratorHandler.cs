using System.Security.Cryptography;
using MediatR;
using exam_system.Common.Services;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class VerifyResetOtpOrchestratorHandler(
    IMediator mediator,
    IOtpService otpService,
    IUnitOfWork unitOfWork) : IRequestHandler<VerifyResetOtpOrchestrator, RequestResponse<VerifyResetOtpResponse>>
{
    private const int MaxAttempts = 5;

    public async Task<RequestResponse<VerifyResetOtpResponse>> Handle(
        VerifyResetOtpOrchestrator request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 1. Fetch latest unused OTP for this email via GetResetOtpQuery
        var latestOtp = await mediator.Send(new GetResetOtpQuery(normalizedEmail), cancellationToken);

        if (latestOtp == null)
        {
            return RequestResponse<VerifyResetOtpResponse>.Fail(
                "No active verification code found. Please request a new code.",
                statusCode: 400);
        }

        // 2. Check if code is already locked due to max attempts exceeded
        if (latestOtp.AttemptCount >= MaxAttempts)
        {
            return RequestResponse<VerifyResetOtpResponse>.Fail(
                "Verification code is locked due to too many failed attempts. Please request a new code.",
                statusCode: 400);
        }

        // 3. Check if OTP has expired (distinct error rather than generic invalid-code error)
        if (DateTime.UtcNow > latestOtp.ExpiresAt)
        {
            return RequestResponse<VerifyResetOtpResponse>.Fail(
                "Verification code has expired. Please request a new one.",
                statusCode: 400);
        }

        // 4. Validate OTP hash using constant-time comparison
        var isOtpValid = otpService.VerifyOtp(request.Otp, latestOtp.OtpHash);
        if (!isOtpValid)
        {
            latestOtp.AttemptCount++;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (latestOtp.AttemptCount >= MaxAttempts)
            {
                return RequestResponse<VerifyResetOtpResponse>.Fail(
                    "Verification code is locked due to too many failed attempts. Please request a new code.",
                    statusCode: 400);
            }

            var remainingAttempts = MaxAttempts - latestOtp.AttemptCount;
            return RequestResponse<VerifyResetOtpResponse>.Fail(
                $"Invalid verification code. {remainingAttempts} attempt(s) remaining.",
                statusCode: 400);
        }

        // 5. On correct OTP, issue short-lived reset token (valid for 15 minutes)
        var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

        latestOtp.ResetToken = resetToken;
        latestOtp.ResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);
        latestOtp.OtpHash = string.Empty; // Prevent re-verification of the same OTP

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var responseData = new VerifyResetOtpResponse
        {
            ResetToken = resetToken,
            ExpiresAt = latestOtp.ResetTokenExpiresAt.Value,
            Message = "Verification successful. You can now reset your password."
        };

        return RequestResponse<VerifyResetOtpResponse>.Ok(responseData, "Verification successful");
    }
}
