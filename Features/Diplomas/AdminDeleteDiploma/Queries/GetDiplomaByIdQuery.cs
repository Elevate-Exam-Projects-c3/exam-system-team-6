using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;

public record GetDiplomaByIdQuery(
    Guid DiplomaId
) : IRequest<RequestResponse<GetDiplomaByIdDto>>;