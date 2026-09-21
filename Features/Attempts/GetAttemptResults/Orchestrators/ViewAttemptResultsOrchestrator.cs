using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptResults.Orchestrators;

public record ViewAttemptResultsOrchestrator(
    Guid AttemptId,
    Guid StudentId) : IRequest<RequestResponse<AttemptResultDto>>;
