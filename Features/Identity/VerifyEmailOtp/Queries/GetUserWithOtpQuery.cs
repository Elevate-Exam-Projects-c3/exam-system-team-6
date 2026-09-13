using MediatR;

namespace exam_system.Features.Identity.VerifyEmailOtp.Queries;

public record GetUserWithOtpQuery : IRequest<GetUserWithOtpQueryResult>
{
    public string Email { get; set; } = string.Empty;
}
