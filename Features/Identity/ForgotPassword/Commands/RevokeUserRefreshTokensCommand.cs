using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record RevokeUserRefreshTokensCommand(Guid UserId) : IRequest;
