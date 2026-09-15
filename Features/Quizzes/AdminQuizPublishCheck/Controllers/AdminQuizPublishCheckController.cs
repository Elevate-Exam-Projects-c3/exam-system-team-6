using exam_system.Features.Quizzes.AdminQuizPublishCheck.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
//[Authorize(Roles = "Admin")]
public class AdminQuizPublishCheckController(IMediator mediator) : ControllerBase
{
    [HttpGet("{quizId:guid}/publish-check")]
    public async Task<ActionResult<EndpointResponse<QuizPublishCheckResult>>> GetPublishCheck(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new QuizPublishCheckOrchestrator(id),
            cancellationToken);

        var response = EndpointResponse<QuizPublishCheckResult>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}
