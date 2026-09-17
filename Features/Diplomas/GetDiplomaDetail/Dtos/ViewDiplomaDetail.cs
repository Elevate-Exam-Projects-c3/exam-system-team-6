namespace exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;

public sealed record ViewDiplomaDetailDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public List<ViewDiplomaQuizDto> Quizzes { get; init; } = [];
}