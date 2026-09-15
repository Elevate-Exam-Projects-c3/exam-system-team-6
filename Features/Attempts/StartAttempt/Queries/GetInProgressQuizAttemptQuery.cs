using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries;

public record GetInProgressQuizAttemptQuery(Guid QuizId,Guid StudentId ) : IRequest<RequestResponse<Guid?>>;