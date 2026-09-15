using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Queries;

public record GetQuizQuestionsForPublishCheckQuery(Guid QuizId)
    : IRequest<RequestResponse<List<QuestionForPublishCheckDto>>>;

public record QuestionOptionForPublishCheckDto(Guid Id, bool IsCorrect);

public record QuestionForPublishCheckDto(
    Guid Id,
    string Text,
    int OrderIndex,
    List<QuestionOptionForPublishCheckDto> Options);
