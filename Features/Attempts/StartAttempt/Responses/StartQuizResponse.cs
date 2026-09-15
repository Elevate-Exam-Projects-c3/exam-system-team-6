namespace exam_system.Features.Attempts.StartAttempt.Responses;

public record StartQuizResponse(
    Guid AttemptId,
    DateTime? StartTime = null,
    DateTime? Deadline = null,
    IReadOnlyList<AttemptQuestionResponse>? Questions = null
);

public record AttemptQuestionResponse(
    Guid QuestionId,
    string Text,
    int Order,
    IReadOnlyList<AttemptOptionResponse> Options
);
public record AttemptOptionResponse(
    Guid OptionId,
    string Text
);