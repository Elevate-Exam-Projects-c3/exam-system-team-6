using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Handlers;

public sealed class GetStudentDashboardQueryHandler
    : IRequestHandler<GetStudentDashboardQuery, RequestResponse<StudentDashboardDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IUserContext _userContext;

    public GetStudentDashboardQueryHandler(
        IGenericRepository<Diploma> diplomaRepository,
        IUserContext userContext)
    {
        _diplomaRepository = diplomaRepository;
        _userContext = userContext;
    }

    public async Task<RequestResponse<StudentDashboardDto>> Handle(
        GetStudentDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var diplomas = await _diplomaRepository
            .Get(d =>
                !d.IsDeleted &&
                d.Enrollments.Any(e =>
                    e.StudentId == studentId &&
                    !e.IsDeleted))
            .Select(d => new
            {
                d.Id,
                d.Title,
                d.Description,

                Quizzes = d.Quizzes
                    .Where(q =>
                        !q.IsDeleted &&
                        q.Status == QuizStatus.Published)
                    .Select(q => new
                    {
                        q.Id,
                        q.Title,

                        Attempts = q.Attempts
                            .Where(a =>
                                a.StudentId == studentId &&
                                !a.IsDeleted)
                            .Select(a => new
                            {
                                a.Status,
                                a.Score,
                                a.Passed,
                                a.StartTime,
                                a.SubmittedAt
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);
        
        var diplomaDtos = diplomas
            .Select(d =>
            {
                var totalQuizzes = d.Quizzes.Count;

                var completedQuizzes = d.Quizzes.Count(q =>
                    q.Attempts.Any(a =>
                        a.Status == AttemptStatus.Submitted ||
                        a.Status == AttemptStatus.TimedOut));

                return new DashboardDiplomaDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    TotalQuizzes = totalQuizzes,
                    CompletedQuizzes = completedQuizzes,
                    ProgressPercentage = totalQuizzes == 0
                        ? 0
                        : (double)completedQuizzes / totalQuizzes * 100
                };
            })
            .OrderBy(d => d.Title)
            .ToList();

        var allAttempts = diplomas
            .SelectMany(d => d.Quizzes)
            .SelectMany(q =>
                q.Attempts.Select(a => new
                {
                    QuizId = q.Id,
                    QuizTitle = q.Title,
                    a.Status,
                    a.Score,
                    a.Passed,
                    a.StartTime,
                    a.SubmittedAt
                }))
            .OrderByDescending(a => a.StartTime)
            .ToList();

        var recentAttempts = allAttempts
            .Take(5)
            .Select(a => new RecentAttemptDto
            {
                QuizId = a.QuizId,
                QuizTitle = a.QuizTitle,
                Status = a.Status,
                Score = a.Score,
                Passed = a.Passed,
                StartTime = a.StartTime,
                SubmittedAt = a.SubmittedAt,
                TimeSpentMinutes = a.SubmittedAt.HasValue
                    ? (int)(a.SubmittedAt.Value - a.StartTime).TotalMinutes
                    : 0
            })
            .ToList();

        var completedAttempts = allAttempts
            .Where(a =>
                a.Status == AttemptStatus.Submitted ||
                a.Status == AttemptStatus.TimedOut)
            .ToList();

        var averageScore = completedAttempts
            .Where(a => a.Score.HasValue)
            .Select(a => a.Score!.Value)
            .DefaultIfEmpty()
            .Average();

        var passRate = completedAttempts.Count == 0
            ? 0
            : completedAttempts.Count(a => a.Passed == true)
              / (double)completedAttempts.Count * 100;

        var timeSpentMinutes = completedAttempts
            .Where(a => a.SubmittedAt.HasValue)
            .Sum(a =>
                (int)(a.SubmittedAt!.Value - a.StartTime).TotalMinutes);

        var dashboard = new StudentDashboardDto
        {
            Diplomas = diplomaDtos,
            RecentAttempts = recentAttempts,
            Stats = new DashboardStatsDto
            {
                AverageScore = averageScore,
                PassRate = passRate,
                TimeSpentMinutes = timeSpentMinutes
            }
        };

        return RequestResponse<StudentDashboardDto>.Ok(
            dashboard,
            "Dashboard retrieved successfully.");
    }
}