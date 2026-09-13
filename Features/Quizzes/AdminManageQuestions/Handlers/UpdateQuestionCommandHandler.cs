using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class UpdateQuestionCommandHandler(
    IGenericRepository<Question> questionRepository,
    IGenericRepository<QuestionOption> optionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateQuestionCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetByIdAsync(request.QuestionId, q => q.Options);

        if (question is null || question.QuizId != request.QuizId)
        {
            return RequestResponse.Fail(
                "Question not found",
                404);
        }

        question.Text = request.Text;
        question.Explanation = request.Explanation;
        question.OrderIndex = request.OrderIndex;

        // Full replacement of the option set:
        // soft-delete the old options, then insert the new ones in the same unit of work.
        optionRepository.DeleteRange(question.Options);

        var newOptions = request.Options.Select(o => new QuestionOption
        {
            QuestionId = question.Id,
            OptionText = o.OptionText,
            IsCorrect = o.IsCorrect
        }).ToList();

        await optionRepository.AddRangeAsync(newOptions);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok(
            "Question updated successfully"
        );
    }
}
