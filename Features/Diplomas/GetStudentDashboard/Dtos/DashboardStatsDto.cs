namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos;

public sealed record DashboardStatsDto
{
    public double AverageScore { get; init; }
    public double PassRate { get; init; }
    public int TimeSpentMinutes { get; init; }
}