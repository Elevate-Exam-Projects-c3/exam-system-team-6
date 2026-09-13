using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.VerifyEmailOtp.Queries;

public record GetUserWithOtpQueryResult
{
    public ApplicationUser? User { get; set; }
    public EmailVerificationOtp? LatestUnusedOtp { get; set; }
}
