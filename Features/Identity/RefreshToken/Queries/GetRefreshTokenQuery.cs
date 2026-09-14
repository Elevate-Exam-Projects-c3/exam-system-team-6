using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.RefreshToken.Queries;

public record GetRefreshTokenQuery(string Token) : IRequest<Domain.Entities.Identity.RefreshToken?>;
