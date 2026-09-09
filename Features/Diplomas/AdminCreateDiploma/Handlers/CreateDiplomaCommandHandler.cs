using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.Context;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Handlers;

public class CreateDiplomaCommandHandler
    : IRequestHandler<CreateDiplomaCommand, RequestResponse<Guid>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDiplomaCommandHandler(IGenericRepository<Diploma> diplomaRepository, IUnitOfWork unitOfWork)
    {
        _diplomaRepository = diplomaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<Guid>> Handle(
        CreateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = new Diploma
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
        };

        await _diplomaRepository.AddAsync(diploma);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<Guid>.Created(diploma.Id);
    }
}