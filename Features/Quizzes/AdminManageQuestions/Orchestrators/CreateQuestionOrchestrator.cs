using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;

public record CreateQuestionOrchestrator(
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    IReadOnlyList<QuestionOptionInput> Options
) : IRequest<RequestResponse<Guid>>;

public class CreateQuestionOrchestratorHandler
    : IRequestHandler<CreateQuestionOrchestrator, RequestResponse<Guid>>
{
    private readonly IMediator _mediator;

    public CreateQuestionOrchestratorHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse<Guid>> Handle(
        CreateQuestionOrchestrator request,
        CancellationToken cancellationToken)
    {
        var quizResult = await _mediator.Send(
            new GetQuizByIdQuery(request.QuizId),
            cancellationToken);

        if (!quizResult.Success || quizResult.Data is null)
        {
            return RequestResponse<Guid>.Fail(
                quizResult.Message,
                quizResult.StatusCode);
        }

        var questionResult = await _mediator.Send(
            new CreateQuestionCommand(
                request.QuizId,
                request.Text,
                request.Explanation,
                request.OrderIndex),
            cancellationToken);

        if (!questionResult.Success || questionResult.Data == Guid.Empty)
        {
            return RequestResponse<Guid>.Fail(
                questionResult.Message,
                questionResult.StatusCode);
        }

        var optionsResult = await _mediator.Send(
            new CreateQuestionOptionsCommand(
                questionResult.Data,
                request.Options),
            cancellationToken);

        if (!optionsResult.Success)
        {
            return RequestResponse<Guid>.Fail(
                optionsResult.Message,
                optionsResult.StatusCode);
        }

        return RequestResponse<Guid>.Created(
            questionResult.Data,
            "Question created successfully.");
    }
}
