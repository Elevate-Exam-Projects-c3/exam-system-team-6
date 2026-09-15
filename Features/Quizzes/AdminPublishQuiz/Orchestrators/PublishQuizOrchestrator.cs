using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Quizzes.AdminPublishQuiz.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;

public record PublishQuizOrchestrator(Guid QuizId)
    : IRequest<RequestResponse<PublishQuizResult>>;

public record PublishCheckItem(
    string CheckKey,
    string Description,
    bool Passed,
    Guid? QuestionId = null,
    int? QuestionOrderIndex = null);

public class PublishQuizOrchestratorHandler
    : IRequestHandler<PublishQuizOrchestrator, RequestResponse<PublishQuizResult>>
{
    private readonly IMediator _mediator;

    public PublishQuizOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse<PublishQuizResult>> Handle(
        PublishQuizOrchestrator request,
        CancellationToken cancellationToken)
    {
        var quizResult = await _mediator.Send(
            new CheckQuizExistsQuery(request.QuizId),
            cancellationToken);

        if (!quizResult.Success || quizResult.Data is null)
        {
            return RequestResponse<PublishQuizResult>.Fail(
                quizResult.Message,
                quizResult.StatusCode);
        }

        var questionsResult = await _mediator.Send(
            new GetQuizQuestionsForPublishCheckQuery(request.QuizId),
            cancellationToken);

        if (!questionsResult.Success)
        {
            return RequestResponse<PublishQuizResult>.Fail(
                questionsResult.Message,
                questionsResult.StatusCode);
        }

        var questions = questionsResult.Data ?? new List<QuestionForPublishCheckDto>();

        var checks = new List<PublishCheckItem>
        {
            new(
                "HasAtLeastOneQuestion",
                "Quiz must have at least one question.",
                questions.Count > 0)
        };

        foreach (var question in questions)
        {
            checks.Add(new PublishCheckItem(
                "QuestionTextRequired",
                $"Question #{question.OrderIndex} must have question text.",
                !string.IsNullOrWhiteSpace(question.Text),
                question.Id,
                question.OrderIndex));

            checks.Add(new PublishCheckItem(
                "AtLeastTwoOptions",
                $"Question #{question.OrderIndex} must have at least 2 options.",
                question.Options.Count >= 2,
                question.Id,
                question.OrderIndex));

            checks.Add(new PublishCheckItem(
                "ExactlyOneCorrectOption",
                $"Question #{question.OrderIndex} must have exactly one option marked as correct.",
                question.Options.Count(o => o.IsCorrect) == 1,
                question.Id,
                question.OrderIndex));
        }

        var failedChecks = checks.Where(c => !c.Passed).ToList();

        if (failedChecks.Count > 0)
        {
            var errors = failedChecks
                .GroupBy(c => c.CheckKey)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(c => c.Description).ToArray());

            return RequestResponse<PublishQuizResult>.Fail(
                "Quiz failed the publish readiness checklist.",
                StatusCodes.Status422UnprocessableEntity,
                errors);
        }

        return await _mediator.Send(
            new PublishQuizCommand(request.QuizId),
            cancellationToken);
    }
}
