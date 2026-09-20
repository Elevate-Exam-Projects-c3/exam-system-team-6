using exam_system.Features.Attempts.CheckRemainingTime.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.CheckRemainingTime.Controllers;

[ApiController]
[Route("api/attempts")]
public class GetRemainingTimeController(IMediator mediator) :ControllerBase
{
    [HttpGet("{attemptId}/time-remaining")]
    
    public async Task<IActionResult> GetRemainingTime(
        [FromRoute] Guid attemptId,
        CancellationToken cancellationToken)
    {
        var query = new GetRemainingTimeQuery(attemptId);
        var result = await mediator.Send(query, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<TimeSpan>.FromResult(result)
        );
    }
 
}