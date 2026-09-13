using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public record UpdateQuestionOptionItem(string OptionText, bool IsCorrect);

public record UpdateQuestionCommand(
    Guid QuizId,
    Guid QuestionId,
    string Text,
    string? Explanation,
    int OrderIndex,
    List<UpdateQuestionOptionItem> Options) : IRequest<RequestResponse>;
