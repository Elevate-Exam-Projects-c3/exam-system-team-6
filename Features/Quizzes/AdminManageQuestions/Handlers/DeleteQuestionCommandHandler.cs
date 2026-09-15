using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class DeleteQuestionCommandHandler
    : IRequestHandler<DeleteQuestionCommand, RequestResponse>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(
        IGenericRepository<Question> questionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(
        DeleteQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question is null || question.IsDeleted)
        {
            return RequestResponse.Fail(
                "Question not found.",
                StatusCodes.Status404NotFound);
        }

        await _questionRepository.DeleteAsync(question);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok("Question deleted successfully.");
    }
}
