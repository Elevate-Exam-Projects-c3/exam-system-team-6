using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.CheckRemainingTime.Queries;

public record GetRemainingTimeQuery(Guid AttemptId) : IRequest<RequestResponse<TimeSpan>>;
