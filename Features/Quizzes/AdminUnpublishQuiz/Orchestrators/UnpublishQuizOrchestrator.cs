using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Orchestrators;

public record UnpublishQuizOrchestrator(Guid QuizId)
    : IRequest<RequestResponse<UnpublishQuizResult>>;

public class UnpublishQuizOrchestratorHandler
    : IRequestHandler<UnpublishQuizOrchestrator, RequestResponse<UnpublishQuizResult>>
{
    private readonly IMediator _mediator;

    public UnpublishQuizOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse<UnpublishQuizResult>> Handle(
        UnpublishQuizOrchestrator request,
        CancellationToken cancellationToken)
    {
        var quizResult = await _mediator.Send(
            new CheckQuizExistsQuery(request.QuizId),
            cancellationToken);

        if (!quizResult.Success || quizResult.Data is null)
        {
            return RequestResponse<UnpublishQuizResult>.Fail(
                quizResult.Message,
                quizResult.StatusCode);
        }

        var attemptsResult = await _mediator.Send(
            new CountInProgressAttemptsQuery(request.QuizId),
            cancellationToken);

        if (!attemptsResult.Success)
        {
            return RequestResponse<UnpublishQuizResult>.Fail(
                attemptsResult.Message,
                attemptsResult.StatusCode);
        }

        if (attemptsResult.Data > 0)
        {
            return RequestResponse<UnpublishQuizResult>.Fail(
                $"Cannot unpublish: {attemptsResult.Data} student(s) currently have an in-progress attempt on this quiz.",
                StatusCodes.Status409Conflict);
        }

        return await _mediator.Send(
            new UnpublishQuizCommand(request.QuizId),
            cancellationToken);
    }
}
