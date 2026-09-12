namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Dtos;

public class GetDiplomaByIdDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
}