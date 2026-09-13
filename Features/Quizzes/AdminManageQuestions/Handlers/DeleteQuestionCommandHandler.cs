using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class DeleteQuestionCommandHandler(
    IGenericRepository<Question> questionRepository,
    IGenericRepository<QuestionOption> optionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteQuestionCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetByIdAsync(request.QuestionId, q => q.Quiz, q => q.Options);

        if (question is null || question.QuizId != request.QuizId)
        {
            return RequestResponse.Fail(
                "Question not found",
                404);
        }

        // EXAM-124: questions cannot be deleted from a quiz that is currently published.
        if (question.Quiz.Status == QuizStatus.Published)
        {
            return RequestResponse.Fail(
                "Cannot delete a question from a published quiz. Unpublish the quiz first.",
                409);
        }

        // Soft-delete only: IsDeleted/DeletedAt are set by the repository, the rows stay in the DB.
        questionRepository.Delete(question);
        optionRepository.DeleteRange(question.Options);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok(
            "Question deleted successfully"
        );
    }
}
