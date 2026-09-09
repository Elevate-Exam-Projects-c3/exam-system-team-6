using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators;

public class DeleteDiplomaOrchestrator
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IGenericRepository<StudentEnrollment> _studentEnrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiplomaOrchestrator(
        IGenericRepository<Diploma> diplomaRepository,
        IGenericRepository<StudentEnrollment> studentEnrollmentRepository,
        IUnitOfWork unitOfWork)
    {
        _diplomaRepository = diplomaRepository;
        _studentEnrollmentRepository = studentEnrollmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> ExecuteAsync(
        Guid diplomaId,
        CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetByIdAsync(diplomaId);

        if (diploma is null || diploma.IsDeleted)
        {
            return RequestResponse.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        var hasActiveEnrollments = await _studentEnrollmentRepository
            .Get(e => e.DiplomaId == diplomaId && !e.IsDeleted)
            .AnyAsync(cancellationToken);

        if (hasActiveEnrollments)
        {
            return RequestResponse.Fail(
                "Cannot delete diploma with active student enrollments.",
                StatusCodes.Status409Conflict);
        }

        _diplomaRepository.Delete(diploma);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok(
            "Diploma deleted successfully.");
    }
}