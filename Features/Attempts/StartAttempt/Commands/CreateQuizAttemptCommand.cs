using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Commands;

public record CreateQuizAttemptCommand(Guid QuizId ,Guid StudentId) : IRequest<RequestResponse<Guid>>;