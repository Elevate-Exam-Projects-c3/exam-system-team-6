using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class ReplaceQuestionOptionsCommandHandler
    : IRequestHandler<ReplaceQuestionOptionsCommand, RequestResponse>
{
    private readonly IGenericRepository<QuestionOption> _questionOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReplaceQuestionOptionsCommandHandler(
        IGenericRepository<QuestionOption> questionOptionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionOptionRepository = questionOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(
        ReplaceQuestionOptionsCommand request,
        CancellationToken cancellationToken)
    {
        var existingOptions = await _questionOptionRepository
            .Get(o => o.QuestionId == request.QuestionId && !o.IsDeleted)
            .ToListAsync(cancellationToken);

        if (existingOptions.Count > 0)
        {
            _questionOptionRepository.DeleteRange(existingOptions);
        }

        var newOptions = request.Options
            .Select(o => new QuestionOption
            {
                QuestionId = request.QuestionId,
                OptionText = o.OptionText.Trim(),
                IsCorrect = o.IsCorrect
            })
            .ToList();

        await _questionOptionRepository.AddRangeAsync(newOptions);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse.Ok("Question options replaced successfully.");
    }
}
