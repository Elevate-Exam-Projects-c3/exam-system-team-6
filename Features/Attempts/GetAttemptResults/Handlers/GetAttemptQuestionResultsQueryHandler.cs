using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using exam_system.Features.Attempts.GetAttemptResults.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.GetAttemptResults.Handlers;

public class GetAttemptQuestionResultsQueryHandler(IGenericRepository<QuizAttempt> repository)
    : IRequestHandler<GetAttemptQuestionResultsQuery, List<QuestionResultDto>>
{
    public async Task<List<QuestionResultDto>> Handle(
        GetAttemptQuestionResultsQuery request,
        CancellationToken cancellationToken)
    {
        return await repository.GetAll()
            .Where(a => a.Id == request.AttemptId && !a.IsDeleted)
            .SelectMany(a => a.Answers.Where(x => !x.IsDeleted))
            .OrderBy(x => x.Question.OrderIndex)
            .Select(x => new QuestionResultDto(
                x.QuestionId,
                x.Question.Text,
                x.Question.OrderIndex,
                x.SelectedOptionId,
                x.SelectedOption != null ? x.SelectedOption.OptionText : null,
                x.IsCorrect ?? false,
                x.Question.Options
                    .Where(o => o.IsCorrect && !o.IsDeleted)
                    .Select(o => o.OptionText)
                    .FirstOrDefault(),
                x.Question.Explanation,
                x.Question.Options
                    .Where(o => !o.IsDeleted)
                    .Select(o => new QuestionOptionResultDto(
                        o.Id,
                        o.OptionText,
                        o.IsCorrect,
                        o.Id == x.SelectedOptionId
                    ))
                    .ToList()
            ))
            .ToListAsync(cancellationToken);
    }
}
