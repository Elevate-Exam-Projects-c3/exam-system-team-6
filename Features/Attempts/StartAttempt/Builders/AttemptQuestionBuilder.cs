using exam_system.Domain.Entities.Attempts;
using exam_system.Domain.Entities.Quizzes;

namespace exam_system.Features.Attempts.StartAttempt.Builders;

public class AttemptQuestionBuilder
{
    public List<AttemptQuestion> Build(
        Guid attemptId,
        IReadOnlyList<Guid> questionIds)
    {
        return questionIds
            .OrderBy(_ => Random.Shared.Next())
            .Select((questionId, index) =>
                AttemptQuestion.Create(
                    attemptId,
                    questionId,
                    index + 1))
            .ToList();
    }
}