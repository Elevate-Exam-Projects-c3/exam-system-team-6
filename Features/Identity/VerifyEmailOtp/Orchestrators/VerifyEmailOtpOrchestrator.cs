using MediatR;
using exam_system.Features.Identity.VerifyEmailOtp;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;

public record VerifyEmailOtpOrchestrator : IRequest<RequestResponse<VerifyEmailOtpResponse>>
{
    public string Email { get; init; } = string.Empty;
    public string Otp { get; init; } = string.Empty;
}
