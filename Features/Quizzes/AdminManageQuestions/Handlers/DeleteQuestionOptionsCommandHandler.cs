using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class DeleteQuestionOptionsCommandHandler
    : IRequestHandler<DeleteQuestionOptionsCommand, RequestResponse>
{
    private readonly IGenericRepository<QuestionOption> _questionOptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionOptionsCommandHandler(
        IGenericRepository<QuestionOption> questionOptionRepository,
        IUnitOfWork unitOfWork)
    {
        _questionOptionRepository = questionOptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(
        DeleteQuestionOptionsCommand request,
        CancellationToken cancellationToken)
    {
        var options = await _questionOptionRepository
            .Get(o => o.QuestionId == request.QuestionId && !o.IsDeleted)
            .ToListAsync(cancellationToken);

        if (options.Count > 0)
        {
            _questionOptionRepository.DeleteRange(options);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return RequestResponse.Ok("Question options deleted successfully.");
    }
}
