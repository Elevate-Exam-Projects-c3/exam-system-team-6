using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Commands;

public record PublishQuizCommand(Guid QuizId) : IRequest<RequestResponse<PublishQuizResult>>;

public record PublishQuizResult(Guid QuizId, string Status, DateTime? PublishedAt);
