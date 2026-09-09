using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public class VerifyEmailOtpCommand : IRequest<RequestResponse<VerifyEmailOtpResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}
