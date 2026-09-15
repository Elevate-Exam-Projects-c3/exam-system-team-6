using exam_system.Common.Enums;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public record DeleteQuestionOrchestrator(
    Guid QuizId,
    Guid QuestionId
) : IRequest<RequestResponse>;

public class DeleteQuestionOrchestratorHandler
    : IRequestHandler<DeleteQuestionOrchestrator, RequestResponse>
{
    private readonly IMediator _mediator;

    public DeleteQuestionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse> Handle(
        DeleteQuestionOrchestrator request,
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

        var quizResult = await _mediator.Send(
            new GetQuizByIdQuery(questionResult.Data.QuizId),
            cancellationToken);

        if (!quizResult.Success || quizResult.Data is null)
        {
            return RequestResponse.Fail(
                quizResult.Message,
                quizResult.StatusCode);
        }

        if (quizResult.Data.Status == QuizStatus.Published)
        {
            return RequestResponse.Fail(
                "Cannot delete a question from a published quiz. Unpublish the quiz first.",
                StatusCodes.Status409Conflict);
        }

        var deleteResult = await _mediator.Send(
            new DeleteQuestionCommand(request.QuestionId),
            cancellationToken);

        if (!deleteResult.Success)
        {
            return deleteResult;
        }

        var deleteOptionsResult = await _mediator.Send(
            new DeleteQuestionOptionsCommand(request.QuestionId),
            cancellationToken);

        if (!deleteOptionsResult.Success)
        {
            return deleteOptionsResult;
        }

        return RequestResponse.Ok("Question deleted successfully.");
    }
}
