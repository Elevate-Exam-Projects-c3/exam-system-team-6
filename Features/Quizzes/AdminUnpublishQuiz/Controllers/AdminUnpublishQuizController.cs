using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Controllers;

[ApiController]
[Route("api/admin/quizzes")]
//[Authorize(Roles = "Admin")]
public class AdminUnpublishQuizController(IMediator mediator) : ControllerBase
{
    [HttpPatch("{quizId:guid}/unpublish")]
    public async Task<ActionResult<EndpointResponse<UnpublishQuizResult>>> Unpublish(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UnpublishQuizOrchestrator(id),
            cancellationToken);

        var response = EndpointResponse<UnpublishQuizResult>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}
