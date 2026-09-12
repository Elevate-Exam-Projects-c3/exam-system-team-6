using exam_system.Features.Diplomas.BrowseDiplomas.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Orchestrators;

public record BrowseDiplomasOrchestrator(
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<RequestResponse<PaginatedResult<BrowseDiplomaDto>>>;