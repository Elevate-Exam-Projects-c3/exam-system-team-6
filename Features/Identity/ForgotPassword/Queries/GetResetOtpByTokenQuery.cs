using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Queries;

public record GetResetOtpByTokenQuery(string ResetToken) : IRequest<GetResetOtpByTokenQueryResult?>;

public record GetResetOtpByTokenQueryResult(
    Guid OtpId,
    Guid UserId,
    DateTime? ResetTokenExpiresAt
);
