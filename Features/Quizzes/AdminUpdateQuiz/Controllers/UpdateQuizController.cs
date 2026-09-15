using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Dtos;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Controllers;

[ApiController]
[Route("api/quizzes")]
public class UpdateQuizController(IMediator mediator) : ControllerBase
{
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateQuizDto updateQuizDto,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateQuizCommand(id,updateQuizDto), cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result)
        );
    }
}