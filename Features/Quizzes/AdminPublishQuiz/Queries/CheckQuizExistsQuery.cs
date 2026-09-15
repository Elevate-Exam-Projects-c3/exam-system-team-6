using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Queries;

public record CheckQuizExistsQuery(Guid QuizId) : IRequest<RequestResponse<QuizSummaryDto>>;

public record QuizSummaryDto(Guid Id, string Title, string Status);
