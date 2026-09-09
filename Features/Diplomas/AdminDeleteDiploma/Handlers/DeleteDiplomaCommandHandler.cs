using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers;

public class DeleteDiplomaCommandHandler
    : IRequestHandler<DeleteDiplomaCommand, RequestResponse>
{
    private readonly DeleteDiplomaOrchestrator _orchestrator;

    public DeleteDiplomaCommandHandler(
        DeleteDiplomaOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public async Task<RequestResponse> Handle(
        DeleteDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        return await _orchestrator.ExecuteAsync(
            request.DiplomaId,
            cancellationToken);
    }
}