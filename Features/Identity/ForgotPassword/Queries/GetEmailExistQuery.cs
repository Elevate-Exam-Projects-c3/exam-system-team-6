using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Queries;

public record GetEmailExistQuery(string Email) : IRequest<GetEmailExistQueryResult?>;

public record GetEmailExistQueryResult(
    Guid UserId,
    string FullName,
    string Email,
    DateTime? LatestOtpCreatedAt
);
