using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Queries;

public record GetCompletedQuizCountsQuery(
    IReadOnlyCollection<Guid> DiplomaIds
) : IRequest<RequestResponse<Dictionary<Guid, int>>>;