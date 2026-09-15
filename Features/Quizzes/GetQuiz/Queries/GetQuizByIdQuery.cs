using exam_system.Features.Quizzes.GetQuiz.Responses;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.GetQuiz.Queries;

public record GetQuizByIdQuery(Guid QuizId) : IRequest<RequestResponse<GetQuizQueryDto>>;
