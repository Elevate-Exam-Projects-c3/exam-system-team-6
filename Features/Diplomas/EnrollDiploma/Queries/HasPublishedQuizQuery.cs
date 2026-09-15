using exam_system.Features.Diplomas.EnrollDiploma.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Queries;

public record HasPublishedQuizQuery(
    Guid DiplomaId
) : IRequest<RequestResponse<HasPublishedQuizDto>>;