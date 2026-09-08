namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Dtos;

public class UpdateQuizDto
{
    public string? Title { get; set; } 
    public string? Instructions { get; set; }
    public int? DurationMinutes { get; set; }
    public int? PassScore { get; set; } 
    public int? MaxAttempts { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    
}