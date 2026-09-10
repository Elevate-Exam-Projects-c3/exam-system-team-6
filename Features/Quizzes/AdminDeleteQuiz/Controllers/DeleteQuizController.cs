using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Dtos;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Controllers;

[ApiController]
[Route("api/quizzes")]
public class DeleteQuizController(IMediator mediator) : ControllerBase
{
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteQuizCommand(id), cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result)
        );
    }
}