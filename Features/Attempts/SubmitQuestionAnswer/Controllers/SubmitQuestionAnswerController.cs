using exam_system.Features.Attempts.SubmitQuestionAnswer.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Controllers;

[ApiController]
[Route("api/student/answers")]
[Authorize(Roles = "Student")]
public class SubmitQuestionAnswerController(IMediator mediator) :ControllerBase
{
    [HttpPost("{attemptId}/questions/{questionId}/answer")]
    
    public async Task<IActionResult> SubmitQuestionAnswer(
          [FromRoute] Guid attemptId,
          [FromRoute] Guid questionId,
          [FromQuery] Guid selectedOptionId,
        CancellationToken cancellationToken)
    {
        var orchestrator = new SubmitQuestionAnswerOrchestrator(attemptId, questionId,selectedOptionId);
        var result = await mediator.Send(orchestrator, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result)
        );
    }
 
}