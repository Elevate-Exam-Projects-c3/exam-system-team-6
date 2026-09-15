using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public record DeleteQuestionOptionsCommand(Guid QuestionId) : IRequest<RequestResponse>;
