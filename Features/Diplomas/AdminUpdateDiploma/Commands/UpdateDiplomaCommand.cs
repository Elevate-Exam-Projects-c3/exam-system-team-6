using exam_system.Features.Diplomas.AdminUpdateDiploma.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;

public class UpdateDiplomaCommand : IRequest<RequestResponse<UpdateDiplomaDto>>
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; } = string.Empty;

    public string? Description { get; set; }
}