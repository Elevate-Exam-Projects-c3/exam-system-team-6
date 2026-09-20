namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos;

public sealed record DashboardDiplomaDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int CompletedQuizzes { get; init; }
    public int TotalQuizzes { get; init; }
    public double ProgressPercentage { get; init; }
}