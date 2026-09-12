using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.BrowseDiplomas.Dtos;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Orchestrators;

public class BrowseDiplomasOrchestrator
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IGenericRepository<QuizAttempt> _attemptRepository;
    private readonly IUserContext _userContext;

    public BrowseDiplomasOrchestrator(
        IGenericRepository<Diploma> diplomaRepository,
        IGenericRepository<Quiz> quizRepository,
        IGenericRepository<QuizAttempt> attemptRepository,
        IUserContext userContext)
    {
        _diplomaRepository = diplomaRepository;
        _quizRepository = quizRepository;
        _attemptRepository = attemptRepository;
        _userContext = userContext;
    }

    public async Task<RequestResponse<
        PaginatedResult<BrowseDiplomaDto>>> ExecuteAsync(
        BrowseDiplomasQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId;

        var query = _diplomaRepository
            .GetAll()
            .Where(d =>
                !d.IsDeleted &&
                d.Quizzes.Any(q =>
                    q.Status == QuizStatus.Published &&
                    !q.IsDeleted));

        var totalCount = await query.CountAsync(cancellationToken);

        var diplomas = await query
            .OrderBy(d => d.Title)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new
            {
                d.Id,
                d.Title,
                d.Description,

                TotalQuizzes = d.Quizzes.Count(q =>
                    q.Status == QuizStatus.Published &&
                    !q.IsDeleted)
            })
            .ToListAsync(cancellationToken);

        var diplomaIds = diplomas
            .Select(d => d.Id)
            .ToList();

        var completedQuizCounts = await _attemptRepository
            .Get(a =>
                a.StudentId == studentId &&
                a.Status == AttemptStatus.Submitted &&
                !a.IsDeleted &&
                diplomaIds.Contains(a.Quiz.DiplomaId))
            .GroupBy(a => a.Quiz.DiplomaId)
            .Select(g => new
            {
                DiplomaId = g.Key,
                CompletedQuizzes = g
                    .Select(a => a.QuizId)
                    .Distinct()
                    .Count()
            })
            .ToDictionaryAsync(
                x => x.DiplomaId,
                x => x.CompletedQuizzes,
                cancellationToken);

        var items = diplomas
            .Select(d => new BrowseDiplomaDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                TotalQuizzes = d.TotalQuizzes,
                CompletedQuizzes =
                    completedQuizCounts.TryGetValue(
                        d.Id,
                        out var completed)
                        ? completed
                        : 0
            })
            .ToList();

        var paginatedResult = PaginatedResult<BrowseDiplomaDto>.Create(
            items,
            totalCount,
            request.PageIndex,
            request.PageSize);

        return RequestResponse<
            PaginatedResult<BrowseDiplomaDto>>.Ok(
                paginatedResult);
    }
}