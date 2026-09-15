using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
//[Authorize(Roles = "Admin")]
public class AdminPublishQuizController(IMediator mediator) : ControllerBase
{
    [HttpPatch("{quizId:guid}/publish")]
    public async Task<ActionResult<EndpointResponse<PublishQuizResult>>> Publish(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new PublishQuizOrchestrator(id),
            cancellationToken);

        var response = EndpointResponse<PublishQuizResult>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}
