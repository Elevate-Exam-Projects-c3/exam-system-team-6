using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public record UpdateQuestionOrchestrator(
    Guid QuizId,
    Guid QuestionId,
    string Text,
    string? Explanation,
    int OrderIndex,
    IReadOnlyList<QuestionOptionInput> Options
) : IRequest<RequestResponse>;

public class UpdateQuestionOrchestratorHandler
    : IRequestHandler<UpdateQuestionOrchestrator, RequestResponse>
{
    private readonly IMediator _mediator;

    public UpdateQuestionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse> Handle(
        UpdateQuestionOrchestrator request,
        CancellationToken cancellationToken)
    {
        var questionResult = await _mediator.Send(
            new GetQuestionByIdQuery(request.QuestionId),
            cancellationToken);

        if (!questionResult.Success || questionResult.Data is null)
        {
            return RequestResponse.Fail(
                questionResult.Message,
                questionResult.StatusCode);
        }

        if (questionResult.Data.QuizId != request.QuizId)
        {
            return RequestResponse.Fail(
                "Question not found in this quiz.",
                StatusCodes.Status404NotFound);
        }

        var updateResult = await _mediator.Send(
            new UpdateQuestionCommand(
                request.QuestionId,
                request.Text,
                request.Explanation,
                request.OrderIndex),
            cancellationToken);

        if (!updateResult.Success)
        {
            return updateResult;
        }

        var replaceOptionsResult = await _mediator.Send(
            new ReplaceQuestionOptionsCommand(
                request.QuestionId,
                request.Options),
            cancellationToken);

        if (!replaceOptionsResult.Success)
        {
            return replaceOptionsResult;
        }

        return RequestResponse.Ok("Question updated successfully.");
    }
}
