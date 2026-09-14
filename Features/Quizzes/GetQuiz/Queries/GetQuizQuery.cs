using exam_system.Features.Quizzes.GetQuiz.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.GetQuiz.Queries;

public record GetQuizQueryRequest(Guid QuizId) : IRequest<RequestResponse<GetQuizQueryDto>>;
