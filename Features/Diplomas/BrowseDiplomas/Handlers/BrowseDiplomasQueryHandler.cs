using exam_system.Features.Diplomas.BrowseDiplomas.Dtos;
using exam_system.Features.Diplomas.BrowseDiplomas.Orchestrators;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers;

public class BrowseDiplomasQueryHandler
    : IRequestHandler<
        BrowseDiplomasQuery,
        RequestResponse<PaginatedResult<BrowseDiplomaDto>>>
{
    private readonly BrowseDiplomasOrchestrator _orchestrator;

    public BrowseDiplomasQueryHandler(
        BrowseDiplomasOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public async Task<RequestResponse<
        PaginatedResult<BrowseDiplomaDto>>> Handle(
        BrowseDiplomasQuery request,
        CancellationToken cancellationToken)
    {
        return await _orchestrator.ExecuteAsync(
            request,
            cancellationToken);
    }
}