using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers;

public class ValidateAttemptQuestionHandler( IGenericRepository<AttemptQuestion> attemptQuestionRepository):IRequestHandler<ValidateAttemptQuestionCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(ValidateAttemptQuestionCommand request, CancellationToken cancellationToken)
    {
        var attemptQuestion = await attemptQuestionRepository.Get(
            x => x.AttemptId == request.AttemptId &&
                 x.QuestionId == request.QuestionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (attemptQuestion is null)
        {
            return RequestResponse.Fail(
                "Question does not belong to this attempt.",
                404);
        }

        return RequestResponse.Ok();
    }
}