namespace exam_system.Features.Attempts.GetAttemptResults.Dtos;

public record AttemptResultDto(
    Guid AttemptId,
    Guid QuizId,
    string QuizTitle,
    string Status,
    double Score,
    int PassScore,
    bool Passed,
    DateTime? SubmittedAt,
    int TotalQuestions,
    int CorrectAnswersCount,
    IReadOnlyList<QuestionResultDto> Questions);

public record QuestionResultDto(
    Guid QuestionId,
    string QuestionText,
    int Order,
    Guid? SelectedOptionId,
    string? SelectedAnswer,
    bool IsCorrect,
    string? CorrectAnswer,
    string? Explanation,
    IReadOnlyList<QuestionOptionResultDto>? Options = null);

public record QuestionOptionResultDto(
    Guid OptionId,
    string OptionText,
    bool IsCorrect,
    bool IsSelected);
