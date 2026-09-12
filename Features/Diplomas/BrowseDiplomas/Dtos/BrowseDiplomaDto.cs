namespace exam_system.Features.Diplomas.BrowseDiplomas.Dtos;

public class BrowseDiplomaDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public int CompletedQuizzes { get; init; }

    public int TotalQuizzes { get; init; }
}