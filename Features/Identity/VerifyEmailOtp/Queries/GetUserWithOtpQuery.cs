using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.VerifyEmailOtp.Queries;

public class GetUserWithOtpQuery : IRequest<GetUserWithOtpQueryResult>
{
    public string Email { get; set; } = string.Empty;
}

public class GetUserWithOtpQueryResult
{
    public ApplicationUser? User { get; set; }
    public EmailVerificationOtp? LatestUnusedOtp { get; set; }
}
