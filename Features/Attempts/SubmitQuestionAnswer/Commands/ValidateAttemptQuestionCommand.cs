using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;

public record ValidateAttemptQuestionCommand(Guid AttemptId, Guid QuestionId) : IRequest<RequestResponse>;