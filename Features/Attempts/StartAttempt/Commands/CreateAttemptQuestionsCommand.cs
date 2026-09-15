using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Commands;

public record CreateAttemptQuestionsCommand(Guid AttemptId, IReadOnlyList<Guid> QuestionIds ) : IRequest<RequestResponse<bool>>;