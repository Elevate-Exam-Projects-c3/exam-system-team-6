using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Queries;

public record GetStudentQuizAttemptsCountQuery(Guid QuizId,Guid StudentId) : IRequest<RequestResponse<int>>;