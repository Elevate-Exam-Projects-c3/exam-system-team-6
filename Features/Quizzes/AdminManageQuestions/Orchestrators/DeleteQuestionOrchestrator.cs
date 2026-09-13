using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public record DeleteQuestionOrchestrator(Guid QuizId, Guid QuestionId) : IRequest<RequestResponse>;

public class DeleteQuestionOrchestratorHandler(IMediator mediator)
    : IRequestHandler<DeleteQuestionOrchestrator, RequestResponse>
{
    public async Task<RequestResponse> Handle(DeleteQuestionOrchestrator request, CancellationToken cancellationToken)
    {
        // Step 1: verify the question exists under this quiz (business query, not the repository).
        var question = await mediator.Send(new GetQuestionForManagementQuery(request.QuizId, request.QuestionId), cancellationToken);

        if (question is null)
        {
            return RequestResponse.Fail(
                "Question not found",
                404);
        }

        // Step 2: the publish guard (409) and the soft-delete are enforced inside the command handler.
        return await mediator.Send(
            new DeleteQuestionCommand(request.QuizId, request.QuestionId),
            cancellationToken);
    }
}
