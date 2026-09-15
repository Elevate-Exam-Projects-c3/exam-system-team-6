using exam_system.Common.Enums;
using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Handlers;

public class CountInProgressAttemptsQueryHandler
    : IRequestHandler<CountInProgressAttemptsQuery, RequestResponse<int>>
{
    private readonly IGenericRepository<QuizAttempt> _attemptRepository;

    public CountInProgressAttemptsQueryHandler(IGenericRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<RequestResponse<int>> Handle(
        CountInProgressAttemptsQuery request,
        CancellationToken cancellationToken)
    {
        var inProgressCount = await _attemptRepository.CountAsync(a =>
            a.QuizId == request.QuizId &&
            a.Status == AttemptStatus.InProgress &&
            !a.IsDeleted);

        return RequestResponse<int>.Ok(inProgressCount);
    }
}
