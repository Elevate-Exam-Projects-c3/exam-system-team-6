using exam_system.Features.Diplomas.EnrollDiploma.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Queries;

public record HasActiveEnrollmentQuery(
    Guid DiplomaId
) : IRequest<RequestResponse<HasActiveEnrollmentDto>>;