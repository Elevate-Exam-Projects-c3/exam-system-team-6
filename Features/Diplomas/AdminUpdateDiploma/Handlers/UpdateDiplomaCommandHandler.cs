using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Dtos;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Handlers;

public class UpdateDiplomaCommandHandler : IRequestHandler<UpdateDiplomaCommand, RequestResponse<UpdateDiplomaDto>>
{
    private readonly IMediator _mediator;
    private readonly IGenericRepository<Diploma> _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateDiplomaCommandHandler(IMediator mediator, IGenericRepository<Diploma> repository, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<RequestResponse<UpdateDiplomaDto>> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = await _repository.GetByIdAsync(request.Id);

        if (diploma is null || diploma.IsDeleted)
        {
            return RequestResponse<UpdateDiplomaDto>.Fail(
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

        var response = new UpdateDiplomaDto
        {
            Title = diploma.Title,
            Description = diploma.Description
        };

        return RequestResponse<UpdateDiplomaDto>.Ok(
            response,
            "Diploma updated successfully.");
    }
    
}