using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.BrowseDiplomas.Dtos;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Orchestrators;

public class BrowseDiplomasOrchestratorHandler : IRequestHandler<BrowseDiplomasOrchestrator, RequestResponse<PaginatedResult<BrowseDiplomaDto>>>
{
    private readonly IMediator _mediator;

    public BrowseDiplomasOrchestratorHandler(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse<
        PaginatedResult<BrowseDiplomaDto>>> Handle(
        BrowseDiplomasOrchestrator request,
        CancellationToken cancellationToken)
    {
        var diplomaResult = await _mediator.Send(
            new BrowseDiplomasQuery(
                request.PageIndex,
                request.PageSize),
            cancellationToken);

        if (!diplomaResult.Success)
        {
            return RequestResponse<
                PaginatedResult<BrowseDiplomaDto>>.Fail(
                diplomaResult.Message,
                diplomaResult.StatusCode);
        }

        var diplomaIds = diplomaResult.Data!.Items
            .Select(d => d.Id)
            .ToList();

        var completedQuizResult = await _mediator.Send(
            new GetCompletedQuizCountsQuery(diplomaIds),
            cancellationToken);

        if (!completedQuizResult.Success)
        {
            return RequestResponse<
                PaginatedResult<BrowseDiplomaDto>>.Fail(
                completedQuizResult.Message,
                completedQuizResult.StatusCode);
        }

        foreach (var diploma in diplomaResult.Data.Items)
        {
            diploma.CompletedQuizzes =
                completedQuizResult.Data!
                    .TryGetValue(
                        diploma.Id,
                        out var completed)
                    ? completed
                    : 0;
        }

        return RequestResponse<
            PaginatedResult<BrowseDiplomaDto>>.Ok(
            diplomaResult.Data);
    }
}