using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Handlers;

public class DeleteQuizCommandHandler(IGenericRepository<Quiz> repository, IUnitOfWork unitOfWork) :IRequestHandler<DeleteQuizCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await repository.GetByIdAsync(request.Id);

        if (quiz is null)
        {
            return RequestResponse.Fail(
                "Quiz not found",
                404);
        }
        if (quiz.Status == QuizStatus.Published)
        {
            return RequestResponse.Fail(
                "Published quiz must be unpublished before it can be deleted.",
                409);
        }

        await repository.DeleteAsync(quiz);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok("Quiz deleted successfully");
    }
}