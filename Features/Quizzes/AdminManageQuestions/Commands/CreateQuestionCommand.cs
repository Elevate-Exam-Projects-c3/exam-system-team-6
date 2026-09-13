using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public record CreateQuestionOptionItem(string OptionText, bool IsCorrect);

public record CreateQuestionCommand(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    List<CreateQuestionOptionItem> Options) : IRequest<RequestResponse<Guid>>;

