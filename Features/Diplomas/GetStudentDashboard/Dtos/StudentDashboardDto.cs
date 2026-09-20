namespace exam_system.Features.Diplomas.GetStudentDashboard.Dtos;

public sealed record StudentDashboardDto
{
    public List<DashboardDiplomaDto> Diplomas { get; init; } = [];
    public List<RecentAttemptDto> RecentAttempts { get; init; } = [];
    public DashboardStatsDto Stats { get; init; } = new();
}