using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class CreateQuestionCommandHandler
    : IRequestHandler<CreateQuestionCommand, RequestResponse<Guid>>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuestionCommandHandler(
        IGenericRepository<Question> questionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionRepository = questionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<Guid>> Handle(
        CreateQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var question = new Question
        {
            QuizId = request.QuizId,
            Text = request.Text.Trim(),
            Explanation = string.IsNullOrWhiteSpace(request.Explanation)
                ? null
                : request.Explanation.Trim(),
            OrderIndex = request.OrderIndex
        };

        await _questionRepository.AddAsync(question);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<Guid>.Created(
            question.Id,
            "Question created successfully.");
    }
}
