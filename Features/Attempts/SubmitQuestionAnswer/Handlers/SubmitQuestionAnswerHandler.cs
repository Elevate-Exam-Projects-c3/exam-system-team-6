using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Orchestrators;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers;

public class SubmitQuestionAnswerHandler(IMediator mediator) :IRequestHandler<SubmitQuestionAnswerOrchestrator, RequestResponse>
{
    public async Task<RequestResponse> Handle(SubmitQuestionAnswerOrchestrator request, CancellationToken cancellationToken)
    {
        var deadlineResponse=await mediator.Send(new CheckAttemptDeadlineCommand(request.AttemptId),cancellationToken);
        if(!deadlineResponse.Success)
        {
            return deadlineResponse;
        }
        var questionResponse = await mediator.Send(
            new ValidateAttemptQuestionCommand(
                request.AttemptId,
                request.QuestionId),
            cancellationToken);

        if (!questionResponse.Success)
        {
            return questionResponse;
        }
        
        return await mediator.Send(new SaveQuestionAnswerCommand(
                request.AttemptId,
                request.QuestionId,
                request.SelectedOptionId), cancellationToken);
    }
}