using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class GetStudentQuizAttemptsCountHandler(
    IGenericRepository<QuizAttempt> attemptRepository,
    IUserContext userContext):IRequestHandler<GetStudentQuizAttemptsCountQuery, RequestResponse<int>>
{
    public async Task<RequestResponse<int>> Handle(
        GetStudentQuizAttemptsCountQuery request,
        CancellationToken cancellationToken)
    {
        // var studentId = userContext.UserId;

        var count = await attemptRepository
            .Get(x =>
                x.QuizId == request.QuizId &&
                x.StudentId == request.StudentId &&
                (x.Status == AttemptStatus.Submitted ||
                 x.Status == AttemptStatus.TimedOut))
            .CountAsync(cancellationToken);

        return RequestResponse<int>.Ok(count);
    }
}