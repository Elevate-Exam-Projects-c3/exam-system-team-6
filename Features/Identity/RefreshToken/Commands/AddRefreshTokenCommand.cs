using MediatR;

namespace exam_system.Features.Identity.RefreshToken.Commands;

public record AddRefreshTokenCommand(Guid UserId, string Token, DateTime ExpiresAt) : IRequest<Domain.Entities.Identity.RefreshToken>;
