using exam_system.Domain.Common;
using exam_system.Domain.Entities.Quizzes;

namespace exam_system.Domain.Entities.Attempts;

public class AttemptQuestion :BaseEntity
{
    public Guid Id { get; private set; }

    public Guid AttemptId { get; private set; }
    public QuizAttempt Attempt { get; private set; } = null!;

    public Guid QuestionId { get; private set; }
    public Question Question { get; private set; } = null!;

    public int Order { get; private set; }
    
    public static AttemptQuestion Create(
        Guid attemptId,
        Guid questionId,
        int order)
    {
        return new AttemptQuestion
        {
            Id = Guid.NewGuid(),
            AttemptId = attemptId,
            QuestionId = questionId,
            Order = order
        };
    }
}