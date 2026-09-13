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
    List<UpdateQuestionOptionItem> Options) : IRequest<RequestResponse>;

/// <summary>Body of PUT /api/quizzes/{quizId}/questions/{questionId}.</summary>
public class UpdateQuestionRequest
{
    public string Text { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public int OrderIndex { get; set; }
    public List<UpdateQuestionOptionItem> Options { get; set; } = new();
}

public class UpdateQuestionOrchestratorHandler(IMediator mediator)
    : IRequestHandler<UpdateQuestionOrchestrator, RequestResponse>
{
    public async Task<RequestResponse> Handle(UpdateQuestionOrchestrator request, CancellationToken cancellationToken)
    {
        // Step 1: verify the question exists under this quiz (business query, not the repository).
        var question = await mediator.Send(new GetQuestionForManagementQuery(request.QuizId, request.QuestionId), cancellationToken);

        if (question is null)
        {
            return RequestResponse.Fail(
                "Question not found",
                404);
        }

        // Step 2: update the question and replace its options.
        return await mediator.Send(
            new UpdateQuestionCommand(request.QuizId, request.QuestionId, request.Text, request.Explanation, request.OrderIndex, request.Options),
            cancellationToken);
    }
}
