using exam_system.Features.Attempts.GetAttemptResults.Dtos;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptResults.Queries;

public record GetAttemptQuestionResultsQuery(Guid AttemptId) : IRequest<List<QuestionResultDto>>;
