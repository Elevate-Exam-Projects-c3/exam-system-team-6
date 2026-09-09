using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers;

public class DeleteDiplomaCommandHandler : IRequestHandler<DeleteDiplomaCommand, RequestResponse>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IGenericRepository<StudentEnrollment> _studentEnrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    
    public DeleteDiplomaCommandHandler(
        IGenericRepository<Diploma> diplomaRepository,
        IGenericRepository<StudentEnrollment> studentEnrollmentRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _diplomaRepository = diplomaRepository;
        _studentEnrollmentRepository = studentEnrollmentRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }   
    
    public async Task<RequestResponse> Handle(DeleteDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetByIdAsync(request.DiplomaId);
        if (diploma == null)
        {
            return RequestResponse.Fail("Diploma not found.", StatusCodes.Status404NotFound);
        }

        var hasActiveEnrollments = await _studentEnrollmentRepository
            .Get(e => e.DiplomaId == request.DiplomaId &&
                      !e.IsDeleted)
            .AnyAsync(cancellationToken);
        if (hasActiveEnrollments)
        {
            return RequestResponse.Fail("Cannot delete diploma with active student enrollments.",StatusCodes.Status409Conflict);
        }

        _diplomaRepository.Delete(diploma);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok("Diploma deleted successfully.");
    }
}