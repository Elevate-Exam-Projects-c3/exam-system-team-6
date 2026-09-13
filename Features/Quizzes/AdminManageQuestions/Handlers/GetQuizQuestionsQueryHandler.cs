using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetQuizQuestionsQueryHandler(IGenericRepository<Question> questionRepository)
    : IRequestHandler<GetQuizQuestionsQuery, RequestResponse<IReadOnlyList<QuestionResponse>>>
{
    public async Task<RequestResponse<IReadOnlyList<QuestionResponse>>> Handle(GetQuizQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = await questionRepository
            .Get(q => q.QuizId == request.QuizId)
            .Include(q => q.Options)
            .OrderBy(q => q.OrderIndex)
            .ToListAsync(cancellationToken);

        var result = questions
            .Select(QuestionResponse.FromEntity)
            .ToList();

        return RequestResponse<IReadOnlyList<QuestionResponse>>.Ok(
            result,
            "Success"
        );
    }
}
