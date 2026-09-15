using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries;

public record GetQuestionByIdQuery(
    Guid QuestionId
) : IRequest<RequestResponse<QuestionData>>;

public record QuestionData(
    Guid Id,
    Guid QuizId,
    string Text
);
