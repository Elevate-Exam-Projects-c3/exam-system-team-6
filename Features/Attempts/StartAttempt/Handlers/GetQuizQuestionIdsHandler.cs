using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class GetQuizQuestionIdsHandler(IGenericRepository<Quiz> quizRepository) : IRequestHandler<GetQuizQuestionIdsQuery, RequestResponse<IReadOnlyList<Guid>>>
{
    public async Task<RequestResponse<IReadOnlyList<Guid>>> Handle(
        GetQuizQuestionIdsQuery request,
        CancellationToken cancellationToken)
    {
        var questionIds = await quizRepository
            .Get(x => x.Id == request.QuizId)
            .SelectMany(x => x.Questions.Select(q => q.Id))
            .ToListAsync(cancellationToken);

        return RequestResponse<IReadOnlyList<Guid>>.Ok(questionIds);
    }
}