using exam_system.Features.Attempts.StartAttempt.Responses;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries;

public record GetAttemptQuestionsQuery(Guid AttemptId) : IRequest<RequestResponse<IReadOnlyList<AttemptQuestionResponse>>>;