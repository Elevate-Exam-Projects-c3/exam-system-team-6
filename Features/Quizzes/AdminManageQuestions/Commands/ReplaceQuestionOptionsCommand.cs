using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public record ReplaceQuestionOptionsCommand(
    Guid QuestionId,
    IReadOnlyList<QuestionOptionInput> Options
) : IRequest<RequestResponse>;
