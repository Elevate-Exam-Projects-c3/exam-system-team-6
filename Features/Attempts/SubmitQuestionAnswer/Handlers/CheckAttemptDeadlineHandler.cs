using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers;

public class CheckAttemptDeadlineHandler(IGenericRepository<QuizAttempt> attemptRepository,
    IUnitOfWork unitOfWork) :IRequestHandler<CheckAttemptDeadlineCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(CheckAttemptDeadlineCommand request, CancellationToken cancellationToken)
    {
        var attempt = await attemptRepository.GetByIdAsync(
            request.AttemptId);

        if (attempt is null)
        {
            return RequestResponse.Fail("Attempt not found",404);
        }

        if (DateTime.UtcNow <= attempt.Deadline)
        {
            return RequestResponse.Ok();
        }

        attempt.MarkAsTimedOut();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Fail(
            "The attempt deadline has expired.",
            410);   
    }
}