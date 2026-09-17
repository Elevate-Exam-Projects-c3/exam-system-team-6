namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;

public sealed record ViewDiplomaQuizDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public int DurationMinutes { get; init; }

    public int PassScore { get; init; }

    public int? MaxAttempts { get; init; }

    public bool CanAttempt { get; init; }

    public bool IsResumable { get; init; }
}