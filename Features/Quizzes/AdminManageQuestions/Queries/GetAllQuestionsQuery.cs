using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries;

public record GetAllQuestionsQuery(
    Guid QuizId
) : IRequest<RequestResponse<List<QuestionListItemData>>>;

public record QuestionListItemData(
    Guid Id,
    string Text,
    string? Explanation,
    int OrderIndex
);
