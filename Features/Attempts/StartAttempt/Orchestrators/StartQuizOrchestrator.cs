using exam_system.Features.Attempts.StartAttempt.Responses;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Orchestrators;

public record StartQuizOrchestrator(Guid QuizId,Guid StudentId):IRequest<RequestResponse<StartQuizResponse>>;