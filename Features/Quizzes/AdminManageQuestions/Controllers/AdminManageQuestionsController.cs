using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Controllers;

[ApiController]
[Route("api/quizzes/{quizId:guid}/questions")]
public class AdminManageQuestionsController(IMediator mediator) : ControllerBase
{
    // EXAM-123 — Admin-only POST
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid quizId,
        CreateQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new AddQuestionOrchestrator(quizId, request.Text, request.Explanation, request.OrderIndex, request.Options),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<Guid>.FromResult(result)
        );
    }

    // EXAM-123 — Admin-only PUT (question text + full option set replacement)
    [HttpPut("{questionId:guid}")]
    public async Task<IActionResult> Update(
        Guid quizId,
        Guid questionId,
        UpdateQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new UpdateQuestionOrchestrator(quizId, questionId, request.Text, request.Explanation, request.OrderIndex, request.Options),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result)
        );
    }

    // EXAM-124 — DELETE with publish guard (409) and soft-delete
    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> Delete(
        Guid quizId,
        Guid questionId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteQuestionOrchestrator(quizId, questionId),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result)
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid quizId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetQuizQuestionsQuery(quizId),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<IReadOnlyList<QuestionResponse>>.FromResult(result)
        );
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetById(
        Guid quizId,
        Guid questionId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetQuestionDetailQuery(quizId, questionId),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<QuestionResponse>.FromResult(result)
        );
    }
}
