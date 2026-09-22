namespace exam_system.Features.Attempts.GetAttemptHistory.Dtos;

public record StudentAttemptHistoryDto(
    Guid AttemptId,
    Guid QuizId,
    string QuizTitle,
    string Status,
    double? Score,
    int PassScore,
    bool? Passed,
    DateTime StartTime,
    DateTime? SubmittedAt);
