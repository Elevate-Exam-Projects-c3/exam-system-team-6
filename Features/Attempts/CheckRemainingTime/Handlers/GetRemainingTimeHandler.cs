using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.CheckRemainingTime.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.CheckRemainingTime.Handlers;

public class GetRemainingTimeHandler(IGenericRepository<QuizAttempt> attemptRepository,IUnitOfWork unitOfWork) : IRequestHandler<GetRemainingTimeQuery, RequestResponse<TimeSpan>>
{
    public async Task<RequestResponse<TimeSpan>> Handle(GetRemainingTimeQuery request, CancellationToken cancellationToken)
    {
        var attempt = await attemptRepository.GetByIdAsync(request.AttemptId);

        if (attempt is null)
        {
            return RequestResponse<TimeSpan>.Fail("Attempt not found",404);
        }
        var now = DateTime.UtcNow;
        if(now >= attempt.Deadline)
        {
            attempt.MarkAsTimedOut();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return RequestResponse<TimeSpan>.Fail("The attempt deadline has expired.",410);
        }
        return RequestResponse<TimeSpan>.Ok(attempt.Deadline - now);
        
    }
}