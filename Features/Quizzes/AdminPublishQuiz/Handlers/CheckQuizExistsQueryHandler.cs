using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminPublishQuiz.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers;

public class CheckQuizExistsQueryHandler
    : IRequestHandler<CheckQuizExistsQuery, RequestResponse<QuizSummaryDto>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public CheckQuizExistsQueryHandler(IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse<QuizSummaryDto>> Handle(
        CheckQuizExistsQuery request,
        CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository
            .Get(q => q.Id == request.QuizId && !q.IsDeleted)
            .Select(q => new QuizSummaryDto(q.Id, q.Title, q.Status.ToString()))
            .FirstOrDefaultAsync(cancellationToken);

        if (quiz is null)
        {
            return RequestResponse<QuizSummaryDto>.Fail(
                "Quiz not found.",
                StatusCodes.Status404NotFound);
        }

        return RequestResponse<QuizSummaryDto>.Ok(quiz);
    }
}
