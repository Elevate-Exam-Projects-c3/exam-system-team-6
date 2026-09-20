using exam_system.Common.Enums;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos;

public sealed record RecentAttemptDto
{
    public Guid QuizId { get; init; }
    public string QuizTitle { get; init; } = string.Empty;
    public AttemptStatus Status { get; init; }
    public double? Score { get; init; }
    public bool? Passed { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime? SubmittedAt { get; init; }
    public int TimeSpentMinutes { get; init; }
}