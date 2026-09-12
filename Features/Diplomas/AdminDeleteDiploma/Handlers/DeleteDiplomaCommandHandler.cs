using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers;

public class DeleteDiplomaCommandHandler
    : IRequestHandler<DeleteDiplomaCommand, RequestResponse>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiplomaCommandHandler(
        IGenericRepository<Diploma> diplomaRepository,
        IUnitOfWork unitOfWork)
    {
        _diplomaRepository = diplomaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(
        DeleteDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetByIdAsync(
            request.DiplomaId);

        if (diploma is null || diploma.IsDeleted)
        {
            return RequestResponse.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        _diplomaRepository.Delete(diploma);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok(
            "Diploma deleted successfully.");
    }
}