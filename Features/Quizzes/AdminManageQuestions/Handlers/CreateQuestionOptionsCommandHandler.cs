using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class CreateQuestionOptionsCommandHandler
    : IRequestHandler<CreateQuestionOptionsCommand, RequestResponse>
{
    private readonly IGenericRepository<QuestionOption> _questionOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuestionOptionsCommandHandler(
        IGenericRepository<QuestionOption> questionOptionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionOptionRepository = questionOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(
        CreateQuestionOptionsCommand request,
        CancellationToken cancellationToken)
    {
        var options = request.Options
            .Select(o => new QuestionOption
            {
                QuestionId = request.QuestionId,
                OptionText = o.OptionText.Trim(),
                IsCorrect = o.IsCorrect
            })
            .ToList();

        await _questionOptionRepository.AddRangeAsync(options);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok("Question options created successfully.");
    }
}
