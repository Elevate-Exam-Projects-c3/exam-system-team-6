using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;

public class EnrollDiplomaOrchestrator
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IGenericRepository<StudentEnrollment> _enrollmentRepository;
    private readonly IGenericRepository<Quiz>  _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public EnrollDiplomaOrchestrator
    (IGenericRepository<Diploma> diplomaRepository,
        IGenericRepository<Quiz> quizRepository,
        IGenericRepository<StudentEnrollment> enrollmentRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _diplomaRepository = diplomaRepository;
        _quizRepository = quizRepository;
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    
    public async Task<RequestResponse<Guid>> ExecuteAsync(
        EnrollDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var diploma = await _diplomaRepository.GetByIdAsync(
            request.DiplomaId);

        if (diploma is null || diploma.IsDeleted)
        {
            return RequestResponse<Guid>.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        var hasPublishedQuiz = await _quizRepository
            .Get(q =>
                q.DiplomaId == request.DiplomaId &&
                q.Status == QuizStatus.Published &&
                !q.IsDeleted)
            .AnyAsync(cancellationToken);

        if (!hasPublishedQuiz)
        {
            return RequestResponse<Guid>.Fail(
                "Cannot enroll in a diploma without a published quiz.",
                StatusCodes.Status409Conflict);
        }

        var alreadyEnrolled = await _enrollmentRepository
            .Get(e =>
                e.StudentId == studentId &&
                e.DiplomaId == request.DiplomaId &&
                !e.IsDeleted)
            .AnyAsync(cancellationToken);

        if (alreadyEnrolled)
        {
            return RequestResponse<Guid>.Fail(
                "Student is already enrolled in this diploma.",
                StatusCodes.Status409Conflict);
        }

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