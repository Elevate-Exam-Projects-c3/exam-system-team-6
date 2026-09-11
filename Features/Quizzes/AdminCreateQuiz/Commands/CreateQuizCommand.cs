using exam_system.Features.Quizzes.AdminCreateQuiz.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Commands;

public record CreateQuizCommand(CreateQuizDto CreateQuizDto) : IRequest<RequestResponse<Guid>>;