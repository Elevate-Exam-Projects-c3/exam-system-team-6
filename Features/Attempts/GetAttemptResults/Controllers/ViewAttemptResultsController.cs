using System.Security.Claims;
using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Attempts.GetAttemptResults.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.GetAttemptResults.Controllers;

[ApiController]
[Route("api/attempts")]
[Authorize(Roles = "Student")]
public class ViewAttemptResultsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{attemptId:guid}/results")]
    public async Task<IActionResult> GetResults(
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var callerUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var orchestrator = new ViewAttemptResultsOrchestrator(attemptId, callerUserId);
        var result = await mediator.Send(orchestrator, cancellationToken);
        var response = EndpointResponse<AttemptResultDto>.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }
}
