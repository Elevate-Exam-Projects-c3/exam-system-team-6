using System.Security.Claims;
using exam_system.Features.Attempts.GetAttemptHistory.Dtos;
using exam_system.Features.Attempts.GetAttemptHistory.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Attempts.GetAttemptHistory.Controllers;

[ApiController]
[Route("api/attempts")]
[Authorize(Roles = "Student")]
public class GetAttemptHistoryController(IMediator mediator) : ControllerBase
{
    [HttpGet("my-history")]
    public async Task<IActionResult> GetMyAttemptHistory(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var callerUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetAttemptHistoryQuery(pageIndex, pageSize, callerUserId);
        var result = await mediator.Send(query, cancellationToken);
        var response = EndpointResponse<PaginatedResult<StudentAttemptHistoryDto>>.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }
}
