using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public record AddQuestionOrchestrator(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    List<CreateQuestionOptionItem> Options) : IRequest<RequestResponse<Guid>>;

/// <summary>Body of POST /api/quizzes/{quizId}/questions.</summary>
public class CreateQuestionRequest
{
    public string Text { get; set; } = string.Empty;
    public string? Explanation { get; set; }
    public int OrderIndex { get; set; }
    public List<CreateQuestionOptionItem> Options { get; set; } = new();
}

public class AddQuestionOrchestratorHandler(IMediator mediator)
    : IRequestHandler<AddQuestionOrchestrator, RequestResponse<Guid>>
{
    public async Task<RequestResponse<Guid>> Handle(AddQuestionOrchestrator request, CancellationToken cancellationToken)
    {
        // Step 1: verify the parent quiz exists (business query, not the repository).
        var quiz = await mediator.Send(new GetQuizForQuestionManagementQuery(request.QuizId), cancellationToken);

        if (quiz is null)
        {
            return RequestResponse<Guid>.Fail(
                "Quiz not found",
                404);
        }

        // Step 2: create the question with its options.
        return await mediator.Send(
            new CreateQuestionCommand(request.QuizId, request.Text, request.Explanation, request.OrderIndex, request.Options),
            cancellationToken);
    }
}
