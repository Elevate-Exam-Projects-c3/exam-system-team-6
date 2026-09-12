using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Handlers;

public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommand, RequestResponse<Guid>>
{
    private readonly IGenericRepository<Diploma> _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateDiplomaCommandHandler(IGenericRepository<Diploma> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<RequestResponse<Guid>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = await _repository.GetByIdAsync(request.Id);

        if (diploma is null || diploma.IsDeleted)
        {
            return RequestResponse<Guid>.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        if (request.Title is not null)
        {
            diploma.Title = request.Title.Trim();
        }

        if (request.Description is not null)
        {
            diploma.Description = request.Description.Trim();
        }
        
        diploma.UpdatedAt = DateTime.UtcNow;

        _repository.Update(diploma);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<Guid>.Ok(
            diploma.Id,
            "Diploma updated successfully.");
    }
    
}