using exam_system.Common.Enums;
using MediatR;

namespace exam_system.Features.Attempts.GetAttemptResults.Queries;

public record GetAttemptSummaryQuery(Guid AttemptId) : IRequest<AttemptSummaryDto?>;

public record AttemptSummaryDto(
    Guid Id,
    Guid StudentId,
    Guid StudentUserId,
    Guid QuizId,
    string QuizTitle,
    int PassScore,
    AttemptStatus Status,
    double? Score,
    bool? Passed,
    DateTime? SubmittedAt);
