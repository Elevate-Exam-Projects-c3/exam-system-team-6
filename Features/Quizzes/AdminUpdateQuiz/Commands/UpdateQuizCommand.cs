using exam_system.Features.Quizzes.AdminUpdateQuiz.Dtos;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;

public record UpdateQuizCommand(Guid Id,UpdateQuizDto UpdateQuizDto) : IRequest<RequestResponse>;