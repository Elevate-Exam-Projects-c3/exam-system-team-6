using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands;

public class CreateDiplomaCommand : IRequest<RequestResponse<Guid>>
{
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}