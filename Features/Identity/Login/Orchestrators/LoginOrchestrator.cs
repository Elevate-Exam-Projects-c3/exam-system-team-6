using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Login.Orchestrators;

public class LoginOrchestrator : IRequest<RequestResponse<LoginResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
