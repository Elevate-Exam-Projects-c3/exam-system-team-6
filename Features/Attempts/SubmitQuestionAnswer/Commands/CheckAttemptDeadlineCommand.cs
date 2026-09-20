using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;

public record CheckAttemptDeadlineCommand(Guid AttemptId): IRequest<RequestResponse>;