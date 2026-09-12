using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Dtos;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers;

public class HasActiveEnrollmentsQueryHandler
    : IRequestHandler<
        HasActiveEnrollmentsQuery,
        RequestResponse<HasActiveEnrollmentsDto>>
{
    private readonly IGenericRepository<StudentEnrollment>
        _studentEnrollmentRepository;

    public HasActiveEnrollmentsQueryHandler(
        IGenericRepository<StudentEnrollment> studentEnrollmentRepository)
    {
        _studentEnrollmentRepository = studentEnrollmentRepository;
    }

    public async Task<RequestResponse<HasActiveEnrollmentsDto>> Handle(
        HasActiveEnrollmentsQuery request,
        CancellationToken cancellationToken)
    {
        var hasActiveEnrollments = await _studentEnrollmentRepository
            .Get(e =>
                e.DiplomaId == request.DiplomaId &&
                !e.IsDeleted)
            .AnyAsync(cancellationToken);

        var data = new HasActiveEnrollmentsDto
        {
            HasActiveEnrollments = hasActiveEnrollments
        };

        return RequestResponse<HasActiveEnrollmentsDto>.Ok(data);
    }
}