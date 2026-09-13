using exam_system.Domain.Entities.Quizzes;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries;

/// <summary>Internal lookup used by orchestrators to verify the question exists under the given quiz.</summary>
public record GetQuestionForManagementQuery(Guid QuizId, Guid QuestionId) : IRequest<Question?>;
