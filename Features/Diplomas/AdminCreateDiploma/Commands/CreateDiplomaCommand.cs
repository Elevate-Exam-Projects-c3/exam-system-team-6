using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Commands;

public record CreateDiplomaCommand(
    string Title,
    string? Description
) : IRequest<RequestResponse<Guid>>;