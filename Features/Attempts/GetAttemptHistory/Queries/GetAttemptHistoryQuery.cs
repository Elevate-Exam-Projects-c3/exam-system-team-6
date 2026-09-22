using exam_system.Features.Attempts.GetAttemptHistory.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptHistory.Queries;

public record GetAttemptHistoryQuery(
    int PageIndex = 1,
    int PageSize = 10,
    Guid StudentId = default)
    : IRequest<RequestResponse<PaginatedResult<StudentAttemptHistoryDto>>>;
