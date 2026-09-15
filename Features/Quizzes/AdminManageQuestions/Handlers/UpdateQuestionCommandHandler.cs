using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class UpdateQuestionCommandHandler
    : IRequestHandler<UpdateQuestionCommand, RequestResponse>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionCommandHandler(
        IGenericRepository<Question> questionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(
        UpdateQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question is null || question.IsDeleted)
        {
            return RequestResponse.Fail(
                "Question not found.",
                StatusCodes.Status404NotFound);
        }

        question.Text = request.Text.Trim();
        question.Explanation = string.IsNullOrWhiteSpace(request.Explanation)
            ? null
            : request.Explanation.Trim();
        question.OrderIndex = request.OrderIndex;
        question.UpdatedAt = DateTime.UtcNow;

        _questionRepository.Update(question);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok("Question updated successfully.");
    }
}
