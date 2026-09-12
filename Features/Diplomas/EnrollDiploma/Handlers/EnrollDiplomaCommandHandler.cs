using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers;

public class EnrollDiplomaCommandHandler
    : IRequestHandler<
        EnrollDiplomaCommand,
        RequestResponse<Guid>>
{
    private readonly IGenericRepository<StudentEnrollment>
        _enrollmentRepository;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public EnrollDiplomaCommandHandler(
        IGenericRepository<StudentEnrollment> enrollmentRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<RequestResponse<Guid>> Handle(
        EnrollDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var enrollment = new StudentEnrollment
        {
            StudentId = studentId,
            DiplomaId = request.DiplomaId,
            EnrolledAt = DateTime.UtcNow
        };

        await _enrollmentRepository.AddAsync(enrollment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<Guid>.Created(
            enrollment.Id,
            "Student enrolled successfully.");
    }
}