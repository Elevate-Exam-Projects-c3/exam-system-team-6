using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Dtos;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers;

public class GetDiplomaForEnrollmentQueryHandler
    : IRequestHandler<
        GetDiplomaForEnrollmentQuery,
        RequestResponse<GetDiplomaForEnrollmentDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public GetDiplomaForEnrollmentQueryHandler(
        IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse<GetDiplomaForEnrollmentDto>> Handle(
        GetDiplomaForEnrollmentQuery request,
        CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository
            .Get(d =>
                d.Id == request.DiplomaId &&
                !d.IsDeleted)
            .Select(d => new GetDiplomaForEnrollmentDto
            {
                Id = d.Id,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (diploma is null)
        {
            return RequestResponse<GetDiplomaForEnrollmentDto>.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        return RequestResponse<GetDiplomaForEnrollmentDto>.Ok(diploma);
    }
}