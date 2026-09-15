using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries;

public record GetQuizQuestionIdsQuery(Guid QuizId) : IRequest<RequestResponse<IReadOnlyList<Guid>>>;