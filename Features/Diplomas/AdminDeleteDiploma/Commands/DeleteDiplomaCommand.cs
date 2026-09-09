using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;

public class DeleteDiplomaCommand : IRequest<RequestResponse>
{
    public Guid DiplomaId { get; set; }
}