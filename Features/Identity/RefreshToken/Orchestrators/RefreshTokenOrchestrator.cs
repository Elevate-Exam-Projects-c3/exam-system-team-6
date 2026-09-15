using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.RefreshToken.Orchestrators;

public record RefreshTokenOrchestrator(string? RefreshToken) : IRequest<RequestResponse<RefreshTokenResponse>>;
