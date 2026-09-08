using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Handlers;

public class UpdateQuizCommandHandler(IGenericRepository<Quiz> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateQuizCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await repository.GetByIdAsync(request.Id);

        if (quiz is null)
        {
            return RequestResponse.Fail(
                "Quiz not found",
                404);
        }
        if (request.UpdateQuizDto.Title is not null)
            quiz.Title = request.UpdateQuizDto.Title;

        if (request.UpdateQuizDto.Instructions is not null)
            quiz.Instructions = request.UpdateQuizDto.Instructions;

        if (request.UpdateQuizDto.DurationMinutes.HasValue)
            quiz.DurationMinutes = request.UpdateQuizDto.DurationMinutes.Value;

        if (request.UpdateQuizDto.PassScore.HasValue)
            quiz.PassScore = request.UpdateQuizDto.PassScore.Value;

        if (request.UpdateQuizDto.MaxAttempts.HasValue)
            quiz.MaxAttempts = request.UpdateQuizDto.MaxAttempts.Value;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok(
            "Quiz updated successfully"
        );
    }
}