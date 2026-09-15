using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Orchestrators;

public class RegisterStudentOrchestrator : IRequest<RequestResponse<RegisterResponse>>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
