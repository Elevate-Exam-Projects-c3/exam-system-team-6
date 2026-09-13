using exam_system.Domain.Entities.Quizzes;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries;

/// <summary>Internal lookup used by orchestrators to verify the parent quiz exists.</summary>
public record GetQuizForQuestionManagementQuery(Guid QuizId) : IRequest<Quiz?>;
