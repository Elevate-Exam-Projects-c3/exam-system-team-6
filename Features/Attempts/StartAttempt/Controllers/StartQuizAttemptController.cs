using exam_system.Features.Attempts.StartAttempt.Orchestrators;
using exam_system.Features.Attempts.StartAttempt.Responses;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.StartAttempt.Controllers;

[ApiController]
[Route("api/quizzes")]
public class StartQuizAttemptController(IMediator mediator) : ControllerBase
{
    [HttpPost("{id}/start-attempt")]
    public async Task<IActionResult> StartQuizAttempt(
        Guid id,
        [FromQuery] Guid studentId,
        CancellationToken cancellationToken)
    {
        var orchestrator = new StartQuizOrchestrator(id, studentId);
        var result = await mediator.Send(orchestrator, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<StartQuizResponse>.FromResult(result)
        );
    }
}
