using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptHistory.Dtos;
using exam_system.Features.Attempts.GetAttemptHistory.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.GetAttemptHistory.Handlers;

public class GetAttemptHistoryQueryHandler(IGenericRepository<QuizAttempt> repository)
    : IRequestHandler<GetAttemptHistoryQuery, RequestResponse<PaginatedResult<StudentAttemptHistoryDto>>>
{
    public async Task<RequestResponse<PaginatedResult<StudentAttemptHistoryDto>>> Handle(
        GetAttemptHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = repository.GetAll()
            .Where(a => !a.IsDeleted &&
                        (a.Student.UserId == request.StudentId || a.StudentId == request.StudentId));

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var attempts = await baseQuery
            .OrderByDescending(a => a.StartTime)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new StudentAttemptHistoryDto(
                a.Id,
                a.QuizId,
                a.Quiz.Title,
                a.Status.ToString(),
                a.Score,
                a.Quiz.PassScore,
                a.Passed,
                a.StartTime,
                a.SubmittedAt
            ))
            .ToListAsync(cancellationToken);

        var paginatedResult = PaginatedResult<StudentAttemptHistoryDto>.Create(
            attempts,
            totalCount,
            request.PageIndex,
            request.PageSize);

        return RequestResponse<PaginatedResult<StudentAttemptHistoryDto>>.Ok(
            paginatedResult,
            "Attempt history retrieved successfully.");
    }
}
