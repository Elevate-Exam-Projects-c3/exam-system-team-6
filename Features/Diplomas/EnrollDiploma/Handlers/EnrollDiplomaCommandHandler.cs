using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers;

public class EnrollDiplomaCommandHandler
    : IRequestHandler<EnrollDiplomaCommand, RequestResponse<Guid>>
{
    private readonly EnrollDiplomaOrchestrator _orchestrator;

    public EnrollDiplomaCommandHandler(
        EnrollDiplomaOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public async Task<RequestResponse<Guid>> Handle(
        EnrollDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        return await _orchestrator.ExecuteAsync(
            request,
            cancellationToken);
    }
}