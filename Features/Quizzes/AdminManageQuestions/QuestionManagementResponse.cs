namespace exam_system.Features.Quizzes.AdminManageQuestions;

public record QuestionOptionResponse(Guid Id, string OptionText, bool IsCorrect);

public record QuestionResponse(
    Guid Id,
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    IReadOnlyList<QuestionOptionResponse> Options)
{
    public static QuestionResponse FromEntity(Domain.Entities.Quizzes.Question question)
    {
        return new QuestionResponse(
            question.Id,
            question.QuizId,
            question.Text,
            question.Explanation,
            question.OrderIndex,
            question.Options
                .OrderBy(o => o.OptionText)
                .Select(o => new QuestionOptionResponse(o.Id, o.OptionText, o.IsCorrect))
                .ToList());
    }
}
