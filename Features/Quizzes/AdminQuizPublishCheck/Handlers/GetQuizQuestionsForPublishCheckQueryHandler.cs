using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Handlers;

public class GetQuizQuestionsForPublishCheckQueryHandler
    : IRequestHandler<GetQuizQuestionsForPublishCheckQuery, RequestResponse<List<QuestionForPublishCheckDto>>>
{
    private readonly IGenericRepository<Question> _questionRepository;

    public GetQuizQuestionsForPublishCheckQueryHandler(IGenericRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<RequestResponse<List<QuestionForPublishCheckDto>>> Handle(
        GetQuizQuestionsForPublishCheckQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await _questionRepository
            .Get(q => q.QuizId == request.QuizId && !q.IsDeleted)
            .OrderBy(q => q.OrderIndex)
            .Select(q => new QuestionForPublishCheckDto(
                q.Id,
                q.Text,
                q.OrderIndex,
                q.Options
                    .Where(o => !o.IsDeleted)
                    .Select(o => new QuestionOptionForPublishCheckDto(o.Id, o.IsCorrect))
                    .ToList()))
            .ToListAsync(cancellationToken);

        return RequestResponse<List<QuestionForPublishCheckDto>>.Ok(questions);
    }
}
