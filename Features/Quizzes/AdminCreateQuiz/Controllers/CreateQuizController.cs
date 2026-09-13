using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers;

[ApiController]
[Route("api/quizzes")]
public class CreateQuizController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateQuizCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<Guid>.FromResult(result)
        );
    }
}