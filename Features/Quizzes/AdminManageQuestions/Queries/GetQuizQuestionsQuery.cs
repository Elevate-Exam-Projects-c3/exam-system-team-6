using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries;

public record GetQuizQuestionsQuery(Guid QuizId) : IRequest<RequestResponse<IReadOnlyList<QuestionResponse>>>;
