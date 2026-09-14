namespace exam_system.Features.Quizzes.GetQuiz.Dtos;

public class GetQuizQueryDto
{
    public Guid Id { get; set; }
    public Guid DiplomaId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public int DurationMinutes { get; set; }
    public int PassScore { get; set; }
    public int? MaxAttempts { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
