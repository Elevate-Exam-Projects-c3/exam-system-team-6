using exam_system.Features.Attempts.StartAttempt.Commands;
using exam_system.Features.Attempts.StartAttempt.Orchestrators;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Attempts.StartAttempt.Responses;
using exam_system.Features.Quizzes.GetQuiz.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class StartQuizOrchestratorHandler(IMediator mediator,IUnitOfWork unitOfWork) :IRequestHandler<StartQuizOrchestrator, RequestResponse<StartQuizResponse>>
{
    public async Task<RequestResponse<StartQuizResponse>> Handle(StartQuizOrchestrator request, CancellationToken cancellationToken)
    {
        var quiz = await mediator.Send(new GetQuizByIdQuery(request.QuizId), cancellationToken);
        if (quiz.Data == null)
        {
            return RequestResponse<StartQuizResponse>.Fail("Quiz not found", 404);
        }
        if(quiz.Data!.Status != "Published")
        {
            return RequestResponse<StartQuizResponse>.Fail("Quiz is not published", 409);
        }
        
        var inProgressAttempt = await mediator.Send(new GetInProgressQuizAttemptQuery(request.QuizId,request.StudentId), cancellationToken);
        if (inProgressAttempt.Data != null)
        {
            return RequestResponse<StartQuizResponse>.Ok(
                new StartQuizResponse(AttemptId: inProgressAttempt.Data.Value),
                "There is already an in-progress attempt for this quiz."
            );
        }
        
        var attemptsCount = await mediator.Send(new GetStudentQuizAttemptsCountQuery(request.QuizId,request.StudentId), cancellationToken);
        if (quiz.Data.MaxAttempts.HasValue && attemptsCount.Data >= quiz.Data.MaxAttempts.Value)
        {
            return RequestResponse<StartQuizResponse>.Fail(
                "You have reached the maximum number of attempts for this quiz.",
                403);
        }
        
        var attempt = await mediator.Send(new CreateQuizAttemptCommand(request.QuizId,request.StudentId), cancellationToken);
        var questionIds = await mediator.Send(new GetQuizQuestionIdsQuery(request.QuizId), cancellationToken);
        
        await mediator.Send(new CreateAttemptQuestionsCommand(attempt.Data, questionIds.Data!), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    
        var questionsList = await mediator.Send(
            new GetAttemptQuestionsQuery(attempt.Data), cancellationToken);
        
        return RequestResponse<StartQuizResponse>.Ok(
            new StartQuizResponse(
                AttemptId: attempt.Data,
                Questions: questionsList.Data,
                StartTime: DateTime.UtcNow,
                Deadline: DateTime.UtcNow.AddMinutes(quiz.Data.DurationMinutes)),
            "Quiz started successfully."
        );
    }
}