using MediatR;
using exam_system.Common.Enums;
using exam_system.Common.Services;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Identity.VerifyEmailOtp.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;

public interface IVerifyEmailOtpOrchestrator
{
    Task<RequestResponse<VerifyEmailOtpResponse>> VerifyAsync(VerifyEmailOtpCommand request, CancellationToken cancellationToken);
}

public class VerifyEmailOtpOrchestrator : IVerifyEmailOtpOrchestrator
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public VerifyEmailOtpOrchestrator(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<RequestResponse<VerifyEmailOtpResponse>> VerifyAsync(VerifyEmailOtpCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch user and their latest unused OTP via Query
        var queryResult = await _mediator.Send(new GetUserWithOtpQuery { Email = request.Email }, cancellationToken);
        var user = queryResult.User;
        var latestOtp = queryResult.LatestUnusedOtp;

        if (user == null)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("Invalid email or verification code.", statusCode: 400);
        }

        // 2. Prevent re-verifying an already active account
        if (user.EmailConfirmed && user.AccountStatus == AccountStatus.Active)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("Email is already verified.", statusCode: 400);
        }

        // 3. Find the most recently issued unused OTP
        if (latestOtp == null)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("No active verification code found. Please request a new one.", statusCode: 400);
        }

        // 4. Check if the OTP is locked due to too many failed attempts (>= 5)
        if (latestOtp.AttemptCount >= 5)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("Verification code is locked due to too many failed attempts. Please request a new code.", statusCode: 400);
        }

        // 5. Check if the OTP has expired (distinct error from invalid code)
        if (DateTime.UtcNow > latestOtp.ExpiresAt)
        {
            return RequestResponse<VerifyEmailOtpResponse>.Fail("Verification code has expired. Please request a new one.", statusCode: 400);
        }

        // 6. Validate OTP against stored hash
        var isOtpValid = _passwordHasher.Verify(request.Otp, latestOtp.OtpHash);
        if (!isOtpValid)
        {
            latestOtp.AttemptCount++;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (latestOtp.AttemptCount >= 5)
            {
                return RequestResponse<VerifyEmailOtpResponse>.Fail("Verification code is locked due to too many failed attempts. Please request a new code.", statusCode: 400);
            }

            var remainingAttempts = 5 - latestOtp.AttemptCount;
            return RequestResponse<VerifyEmailOtpResponse>.Fail($"Invalid verification code. {remainingAttempts} attempt(s) remaining.", statusCode: 400);
        }

        // 7. Success: Mark OTP as used and activate account
        latestOtp.IsUsed = true;
        user.EmailConfirmed = true;
        user.AccountStatus = AccountStatus.Active;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var responseData = new VerifyEmailOtpResponse
        {
            Email = user.Email,
            Message = "Email verified successfully. Your account is now active."
        };

        return RequestResponse<VerifyEmailOtpResponse>.Ok(responseData, "Email verified successfully");
    }
}
