using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.Dtos;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers;

public class BrowseDiplomasQueryHandler
    : IRequestHandler<
        BrowseDiplomasQuery,
        RequestResponse<PaginatedResult<BrowseDiplomaDto>>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IUserContext _userContext;

    public BrowseDiplomasQueryHandler(
        IGenericRepository<Diploma> diplomaRepository,
        IUserContext userContext)
    {
        _diplomaRepository = diplomaRepository;
        _userContext = userContext;
    }

    public async Task<RequestResponse<PaginatedResult<BrowseDiplomaDto>>> Handle(
        BrowseDiplomasQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var query = _diplomaRepository
            .GetAll()
            .Where(d =>
                !d.IsDeleted &&
                d.Quizzes.Any(q =>
                    !q.IsDeleted &&
                    q.Status == QuizStatus.Published));

        var totalCount = await query.CountAsync(cancellationToken);

        var diplomas = await query
            .OrderBy(d => d.Title)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new BrowseDiplomaDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,

                TotalQuizzes = d.Quizzes.Count(q =>
                    !q.IsDeleted &&
                    q.Status == QuizStatus.Published),

                CompletedQuizzes = d.Quizzes.Count(q =>
                    !q.IsDeleted &&
                    q.Status == QuizStatus.Published &&
                    q.Attempts.Any(a =>
                        a.StudentId == studentId &&
                        (a.Status == AttemptStatus.Submitted ||
                         a.Status == AttemptStatus.TimedOut)))
            })
            .ToListAsync(cancellationToken);

        var result = PaginatedResult<BrowseDiplomaDto>.Create(
            diplomas,
            totalCount,
            request.PageIndex,
            request.PageSize);

        return RequestResponse<PaginatedResult<BrowseDiplomaDto>>.Ok(result);
    }
}