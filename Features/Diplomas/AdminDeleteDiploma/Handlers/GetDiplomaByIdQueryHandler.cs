using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Dtos;
using exam_system.Features.Diplomas.AdminDeleteDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Handlers;

public class GetDiplomaByIdQueryHandler
    : IRequestHandler<
        GetDiplomaByIdQuery,
        RequestResponse<GetDiplomaByIdDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;

    public GetDiplomaByIdQueryHandler(
        IGenericRepository<Diploma> diplomaRepository)
    {
        _diplomaRepository = diplomaRepository;
    }

    public async Task<RequestResponse<GetDiplomaByIdDto>> Handle(
        GetDiplomaByIdQuery request,
        CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository
            .Get(d => d.Id == request.DiplomaId && !d.IsDeleted)
            .Select(d => new GetDiplomaByIdDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (diploma is null)
        {
            return RequestResponse<GetDiplomaByIdDto>.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        return RequestResponse<GetDiplomaByIdDto>.Ok(diploma);
    }
}