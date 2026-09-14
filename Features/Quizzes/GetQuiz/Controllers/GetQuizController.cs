using exam_system.Features.Quizzes.GetQuiz.Dtos;
using exam_system.Features.Quizzes.GetQuiz.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.GetQuiz.Controllers;

[ApiController]
[Route("api/quizzes")]
public class GetQuizController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuiz(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetQuizQueryRequest(id);
        var result = await mediator.Send(query, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<GetQuizQueryDto>.FromResult(result)
        );
    }
}
