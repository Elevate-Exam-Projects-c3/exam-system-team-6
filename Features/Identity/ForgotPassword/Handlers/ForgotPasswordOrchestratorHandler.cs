using MediatR;
using exam_system.Common.Services;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class ForgotPasswordOrchestratorHandler(
    IMediator mediator,
    IOtpService otpService,
    IEmailService emailService) : IRequestHandler<ForgotPasswordOrchestrator, RequestResponse>
{
    private const string NeutralMessage = "If this email is registered, a code has been sent.";

    public async Task<RequestResponse> Handle(ForgotPasswordOrchestrator request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // 1. Check if email exists via GetEmailExistQuery
        var user = await mediator.Send(new GetEmailExistQuery(normalizedEmail), cancellationToken);
        if (user == null)
        {
            // Neutral response to prevent account enumeration
            return RequestResponse.Ok(NeutralMessage);
        }

        // 2. Enforce 30-second cooldown interval between resend requests for that account
        if (user.LatestOtpCreatedAt.HasValue && DateTime.UtcNow < user.LatestOtpCreatedAt.Value.AddSeconds(30))
        {
            var remainingSeconds = (int)Math.Ceiling((user.LatestOtpCreatedAt.Value.AddSeconds(30) - DateTime.UtcNow).TotalSeconds);
            return RequestResponse.Fail(
                $"Please wait {remainingSeconds} second(s) before requesting another verification code.",
                statusCode: 429);
        }

        // 3. Generate and hash 6-digit numeric OTP (valid for 10 minutes)
        var plainOtp = otpService.GenerateOtp();
        var hashedOtp = otpService.HashOtp(plainOtp);

        var newOtp = new PasswordResetOtp
        {
            Id = Guid.NewGuid(),
            UserId = user.UserId,
            Email = user.Email,
            OtpHash = hashedOtp,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            AttemptCount = 0,
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Persist via dedicated command
        await mediator.Send(new CreatePasswordResetOtpCommand(newOtp), cancellationToken);

        // 5. Send OTP via email
        await emailService.SendEmailAsync(
            user.Email,
            "Password Reset Verification Code",
            $"<p>Hello {user.FullName},</p>" +
            $"<p>Your password reset verification code is: <strong>{plainOtp}</strong></p>" +
            $"<p>This code is valid for exactly 10 minutes. If you did not request a password reset, please ignore this email.</p>"
        );

        return RequestResponse.Ok(NeutralMessage);
    }
}
