using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Commands;

public record EnrollDiplomaCommand(Guid DiplomaId) : IRequest<RequestResponse<Guid>>;
