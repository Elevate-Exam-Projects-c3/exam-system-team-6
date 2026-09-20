using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptResults.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.GetAttemptResults.Handlers;

public class GetAttemptSummaryQueryHandler(IGenericRepository<QuizAttempt> repository)
    : IRequestHandler<GetAttemptSummaryQuery, AttemptSummaryDto?>
{
    public async Task<AttemptSummaryDto?> Handle(
        GetAttemptSummaryQuery request,
        CancellationToken cancellationToken)
    {
        return await repository.GetAll()
            .Where(a => a.Id == request.AttemptId && !a.IsDeleted)
            .Select(a => new AttemptSummaryDto(
                a.Id,
                a.StudentId,
                a.Student.UserId,
                a.Quiz.Id,
                a.Quiz.Title,
                a.Quiz.PassScore,
                a.Status,
                a.Score,
                a.Passed,
                a.SubmittedAt
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
