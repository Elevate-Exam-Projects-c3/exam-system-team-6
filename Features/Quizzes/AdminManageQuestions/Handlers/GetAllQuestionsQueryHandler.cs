using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetAllQuestionsQueryHandler
    : IRequestHandler<GetAllQuestionsQuery, RequestResponse<List<QuestionListItemData>>>
{
    private readonly IGenericRepository<Question> _questionRepository;

    public GetAllQuestionsQueryHandler(
        IGenericRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<RequestResponse<List<QuestionListItemData>>> Handle(
        GetAllQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await _questionRepository
            .Get(q => q.QuizId == request.QuizId && !q.IsDeleted)
            .OrderBy(q => q.OrderIndex)
            .Select(q => new QuestionListItemData(
                q.Id,
                q.Text,
                q.Explanation,
                q.OrderIndex))
            .ToListAsync(cancellationToken);

        return RequestResponse<List<QuestionListItemData>>.Ok(questions);
    }
}
