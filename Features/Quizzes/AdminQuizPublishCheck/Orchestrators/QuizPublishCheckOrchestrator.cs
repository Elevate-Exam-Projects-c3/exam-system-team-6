using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Orchestrators;

public record QuizPublishCheckOrchestrator(Guid QuizId)
    : IRequest<RequestResponse<QuizPublishCheckResult>>;

public record PublishCheckItem(
    string CheckKey,
    string Description,
    bool Passed,
    Guid? QuestionId = null,
    int? QuestionOrderIndex = null);

public record QuizPublishCheckResult(
    Guid QuizId,
    string QuizTitle,
    bool IsReadyToPublish,
    int TotalQuestions,
    List<PublishCheckItem> Checks);

public class QuizPublishCheckOrchestratorHandler
    : IRequestHandler<QuizPublishCheckOrchestrator, RequestResponse<QuizPublishCheckResult>>
{
    private readonly IMediator _mediator;

    public QuizPublishCheckOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse<QuizPublishCheckResult>> Handle(
        QuizPublishCheckOrchestrator request,
        CancellationToken cancellationToken)
    {
        var quizResult = await _mediator.Send(
            new CheckQuizExistsQuery(request.QuizId),
            cancellationToken);

        if (!quizResult.Success || quizResult.Data is null)
        {
            return RequestResponse<QuizPublishCheckResult>.Fail(
                quizResult.Message,
                quizResult.StatusCode);
        }

        var questionsResult = await _mediator.Send(
            new GetQuizQuestionsForPublishCheckQuery(request.QuizId),
            cancellationToken);

        if (!questionsResult.Success)
        {
            return RequestResponse<QuizPublishCheckResult>.Fail(
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

        var isReadyToPublish = checks.All(c => c.Passed);

        var result = new QuizPublishCheckResult(
            quizResult.Data.Id,
            quizResult.Data.Title,
            isReadyToPublish,
            questions.Count,
            checks);

        return RequestResponse<QuizPublishCheckResult>.Ok(result);
    }
}
