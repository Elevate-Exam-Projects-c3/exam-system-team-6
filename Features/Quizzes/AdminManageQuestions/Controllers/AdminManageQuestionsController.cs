using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Controllers;

[ApiController]
[Route("api/admin/quizzes/{quizId:guid}/questions")]
//[Authorize(Roles = "Admin")]
public class AdminManageQuestionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllQuestions(
        Guid quizId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetAllQuestionsQuery(quizId),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<List<QuestionListItemData>>.FromResult(result));
    }

    [HttpGet("{questionId:guid}")]
    public async Task<IActionResult> GetQuestionById(
        Guid quizId,
        Guid questionId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetQuestionByIdQuery(questionId),
            cancellationToken);

        if (result.Success && result.Data is not null && result.Data.QuizId != quizId)
        {
            var notFound = RequestResponse<QuestionData>.Fail(
                "Question not found in this quiz.",
                StatusCodes.Status404NotFound);

            return StatusCode(
                notFound.StatusCode,
                EndpointResponse<QuestionData>.FromResult(notFound));
        }

        return StatusCode(
            result.StatusCode,
            EndpointResponse<QuestionData>.FromResult(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion(
        Guid quizId,
        CreateQuestionOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            request with { QuizId = quizId },
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<Guid>.FromResult(result));
    }

    [HttpPut("{questionId:guid}")]
    public async Task<IActionResult> UpdateQuestion(
        Guid quizId,
        Guid questionId,
        UpdateQuestionOrchestrator request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            request with { QuizId = quizId, QuestionId = questionId },
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result));
    }

    [HttpDelete("{questionId:guid}")]
    public async Task<IActionResult> DeleteQuestion(
        Guid quizId,
        Guid questionId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new DeleteQuestionOrchestrator(quizId, questionId),
            cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse.FromResult(result));
    }
}
