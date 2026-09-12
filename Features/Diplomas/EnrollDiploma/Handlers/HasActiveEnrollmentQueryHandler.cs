using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Dtos;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers;

public class HasActiveEnrollmentQueryHandler
    : IRequestHandler<
        HasActiveEnrollmentQuery,
        RequestResponse<HasActiveEnrollmentDto>>
{
    private readonly IGenericRepository<StudentEnrollment>
        _enrollmentRepository;

    private readonly IUserContext _userContext;

    public HasActiveEnrollmentQueryHandler(
        IGenericRepository<StudentEnrollment> enrollmentRepository,
        IUserContext userContext)
    {
        _enrollmentRepository = enrollmentRepository;
        _userContext = userContext;
    }

    public async Task<RequestResponse<HasActiveEnrollmentDto>> Handle(
        HasActiveEnrollmentQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var hasActiveEnrollment = await _enrollmentRepository
            .Get(e =>
                e.StudentId == studentId &&
                e.DiplomaId == request.DiplomaId &&
                !e.IsDeleted)
            .AnyAsync(cancellationToken);

        return RequestResponse<HasActiveEnrollmentDto>.Ok(
            new HasActiveEnrollmentDto
            {
                HasActiveEnrollment = hasActiveEnrollment
            });
    }
}