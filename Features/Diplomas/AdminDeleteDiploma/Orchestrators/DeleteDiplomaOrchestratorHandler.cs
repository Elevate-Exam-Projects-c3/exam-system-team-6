using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators;

public class DeleteDiplomaOrchestratorHandler
{
    private readonly IMediator _mediator;

    public DeleteDiplomaOrchestratorHandler(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task<RequestResponse> Handle(
        DeleteDiplomaOrchestrator request,
        CancellationToken cancellationToken)
    {
        var diplomaResult = await _mediator.Send(
            new GetDiplomaByIdQuery(request.DiplomaId),
            cancellationToken);

        if (!diplomaResult.Success)
        {
            return RequestResponse.Fail(
                diplomaResult.Message,
                diplomaResult.StatusCode);
        }

        var enrollmentResult = await _mediator.Send(
            new HasActiveEnrollmentsQuery(request.DiplomaId),
            cancellationToken);

        if (!enrollmentResult.Success)
        {
            return RequestResponse.Fail(
                enrollmentResult.Message,
                enrollmentResult.StatusCode);
        }

        if (enrollmentResult.Data?.HasActiveEnrollments == true)
        {
            return RequestResponse.Fail(
                "Cannot delete diploma with active student enrollments.",
                StatusCodes.Status409Conflict);
        }

        return await _mediator.Send(
            new DeleteDiplomaCommand(request.DiplomaId),
            cancellationToken);
    }
}
