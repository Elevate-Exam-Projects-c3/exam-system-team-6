using exam_system.Common.Enums;
using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Attempts.GetAttemptResults.Orchestrators;
using exam_system.Features.Attempts.GetAttemptResults.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptResults.Handlers;

public class ViewAttemptResultsOrchestratorHandler(IMediator mediator)
    : IRequestHandler<ViewAttemptResultsOrchestrator, RequestResponse<AttemptResultDto>>
{
    public async Task<RequestResponse<AttemptResultDto>> Handle(
        ViewAttemptResultsOrchestrator request,
        CancellationToken cancellationToken)
    {
        // 1. Fetch attempt summary
        var summary = await mediator.Send(
            new GetAttemptSummaryQuery(request.AttemptId),
            cancellationToken);

        if (summary == null)
        {
            return RequestResponse<AttemptResultDto>.Fail("Attempt not found.", 404);
        }

        // 2. Student can only view their own attempts
        if (summary.StudentUserId != request.StudentId && summary.StudentId != request.StudentId)
        {
            return RequestResponse<AttemptResultDto>.Fail(
                "You are not authorized to view this attempt's results.",
                403);
        }

        // 3. Status check: Available only once Status = Submitted or TimedOut
        if (summary.Status == AttemptStatus.InProgress)
        {
            return RequestResponse<AttemptResultDto>.Fail(
                "Attempt results are not available while the attempt is in progress.",
                400);
        }

        // 4. Fetch question results via separated dedicated query
        var questionResults = await mediator.Send(
            new GetAttemptQuestionResultsQuery(request.AttemptId),
            cancellationToken);

        // 5. Compute summary metrics
        var totalQuestions = questionResults.Count;
        var correctAnswersCount = questionResults.Count(q => q.IsCorrect);
        var score = summary.Score
            ?? (totalQuestions > 0 ? Math.Round(((double)correctAnswersCount / totalQuestions) * 100.0, 2) : 0.0);
        var passed = summary.Passed
            ?? (score >= summary.PassScore);

        var resultDto = new AttemptResultDto(
            summary.Id,
            summary.QuizId,
            summary.QuizTitle,
            summary.Status.ToString(),
            score,
            summary.PassScore,
            passed,
            summary.SubmittedAt,
            totalQuestions,
            correctAnswersCount,
            questionResults
        );

        return RequestResponse<AttemptResultDto>.Ok(resultDto, "Attempt results retrieved successfully.");
    }
}
