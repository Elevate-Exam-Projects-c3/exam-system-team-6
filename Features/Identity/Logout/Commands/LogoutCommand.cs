using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Logout.Commands;

public record LogoutCommand(string? RefreshToken) : IRequest<RequestResponse>;
