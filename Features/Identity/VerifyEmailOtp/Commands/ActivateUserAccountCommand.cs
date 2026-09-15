using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Commands;

public record ActivateUserAccountCommand : IRequest<bool>
{
    public Guid UserId { get; init; }
    public Guid OtpId { get; init; }
}
