using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Handlers;

public sealed class ViewDiplomaDetailQueryHandler
    : IRequestHandler<
        ViewDiplomaDetailQuery,
        RequestResponse<ViewDiplomaDetailDto>>
{
    private readonly IGenericRepository<Diploma> _diplomaRepository;
    private readonly IUserContext _userContext;

    public ViewDiplomaDetailQueryHandler(
        IGenericRepository<Diploma> diplomaRepository,
        IUserContext userContext)
    {
        _diplomaRepository = diplomaRepository;
        _userContext = userContext;
    }

    public async Task<RequestResponse<ViewDiplomaDetailDto>> Handle(
        ViewDiplomaDetailQuery request,
        CancellationToken cancellationToken)
    {
        var studentId = _userContext.GetUserId();

        var diploma = await _diplomaRepository
            .Get(d =>
                d.Id == request.DiplomaId &&
                !d.IsDeleted)
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
                        q.DurationMinutes,
                        q.PassScore,
                        q.MaxAttempts,

                        Attempts = q.Attempts
                            .Where(a =>
                                a.StudentId == studentId &&
                                !a.IsDeleted)
                            .Select(a => a.Status)
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (diploma is null)
        {
            return RequestResponse<ViewDiplomaDetailDto>.Fail(
                "Diploma not found.",
                StatusCodes.Status404NotFound);
        }

        if (diploma.Quizzes.Count == 0)
        {
            return RequestResponse<ViewDiplomaDetailDto>.Fail(
                "Diploma has no published quizzes.",
                StatusCodes.Status404NotFound);
        }

        var quizzes = diploma.Quizzes
            .Select(q =>
            {
                var isResumable = q.Attempts.Contains(AttemptStatus.InProgress);

                var attemptsCount = q.Attempts.Count;

                var canAttempt =
                    !isResumable &&
                    (q.MaxAttempts is null ||
                     attemptsCount < q.MaxAttempts.Value);

                return new ViewDiplomaQuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    DurationMinutes = q.DurationMinutes,
                    PassScore = q.PassScore,
                    MaxAttempts = q.MaxAttempts,
                    CanAttempt = canAttempt,
                    IsResumable = isResumable
                };
            })
            .ToList();

        var result = new ViewDiplomaDetailDto
        {
            Id = diploma.Id,
            Title = diploma.Title,
            Description = diploma.Description,
            Quizzes = quizzes
        };

        return RequestResponse<ViewDiplomaDetailDto>.Ok(
            result,
            "Diploma retrieved successfully.");
    }
}