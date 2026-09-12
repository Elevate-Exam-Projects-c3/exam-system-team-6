using exam_system.Features.Diplomas.AdminDeleteDiploma.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;

public record HasActiveEnrollmentsQuery(
    Guid DiplomaId
) : IRequest<RequestResponse<HasActiveEnrollmentsDto>>;