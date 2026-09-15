using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class GetInProgressQuizAttemptHandler(
    IGenericRepository<QuizAttempt> attemptRepository,
    IUserContext userContext) :IRequestHandler<GetInProgressQuizAttemptQuery, RequestResponse<Guid?>>
{
    public async Task<RequestResponse<Guid?>> Handle(GetInProgressQuizAttemptQuery request, CancellationToken cancellationToken)
    {
        // var studentId = userContext.UserId;

        var attemptId = await attemptRepository
            .Get(x =>
                x.QuizId == request.QuizId &&
                x.StudentId == request.StudentId &&
                x.Status == AttemptStatus.InProgress)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return RequestResponse<Guid?>.Ok(attemptId);
    }
}