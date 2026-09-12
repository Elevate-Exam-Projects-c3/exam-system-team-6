using MediatR;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Identity.Register.Orchestrators;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Handlers;

public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, RequestResponse<RegisterResponse>>
{
    private readonly RegisterStudentOrchestrator _orchestrator;

    public RegisterStudentCommandHandler(RegisterStudentOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public async Task<RequestResponse<RegisterResponse>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
    {
        return await _orchestrator.RegisterStudentAsync(request, cancellationToken);
    }
}
