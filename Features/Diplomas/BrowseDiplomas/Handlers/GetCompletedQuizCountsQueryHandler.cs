using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers;

public class GetCompletedQuizCountsQueryHandler : IRequestHandler<GetCompletedQuizCountsQuery, RequestResponse<Dictionary<Guid, int>>>
{
    private readonly IGenericRepository<QuizAttempt>
        _attemptRepository;

    private readonly IUserContext _userContext;

    public GetCompletedQuizCountsQueryHandler(
        IGenericRepository<QuizAttempt> attemptRepository,
        IUserContext userContext)
    {
        _attemptRepository = attemptRepository;
        _userContext = userContext;
    }

    public async Task<RequestResponse<
        Dictionary<Guid, int>>> Handle(
        GetCompletedQuizCountsQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var completedQuizCounts = await _attemptRepository
            .Get(a =>
                a.StudentId == studentId &&
                a.Status == AttemptStatus.Submitted &&
                !a.IsDeleted &&
                request.DiplomaIds.Contains(
                    a.Quiz.DiplomaId))
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

        return RequestResponse<
            Dictionary<Guid, int>>.Ok(
            completedQuizCounts);
    }
}