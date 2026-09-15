using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Queries;
using exam_system.Features.Attempts.StartAttempt.Responses;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class GetAttemptQuestionsHandler(IGenericRepository<AttemptQuestion> attemptQuestionRepository):IRequestHandler<GetAttemptQuestionsQuery, RequestResponse<IReadOnlyList<AttemptQuestionResponse>>>
{
    public async Task<RequestResponse<IReadOnlyList<AttemptQuestionResponse>>> Handle(
        GetAttemptQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var questions = await attemptQuestionRepository
            .Get(x => x.AttemptId == request.AttemptId)
            .OrderBy(x => x.Order)
            .Select(x => new AttemptQuestionResponse(
                x.QuestionId,
                x.Question.Text,
                x.Order,
                x.Question.Options
                    .Select(option => new AttemptOptionResponse(
                        option.Id,
                        option.OptionText))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);

        return RequestResponse<IReadOnlyList<AttemptQuestionResponse>>.Ok(questions);
    }
}