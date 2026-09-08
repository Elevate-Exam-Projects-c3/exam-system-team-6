namespace exam_system.Features.Quizzes.AdminCreateQuiz.Dtos;

public class CreateQuizDto
{
    public Guid DiplomaId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public int DurationMinutes { get; set; }
    public int? PassScore { get; set; } 
    public int? MaxAttempts { get; set; }
}