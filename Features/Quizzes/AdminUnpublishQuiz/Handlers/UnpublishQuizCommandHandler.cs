using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Handlers;

public class UnpublishQuizCommandHandler
    : IRequestHandler<UnpublishQuizCommand, RequestResponse<UnpublishQuizResult>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnpublishQuizCommandHandler(
        IGenericRepository<Quiz> quizRepository,
        IUnitOfWork unitOfWork)
    {
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<UnpublishQuizResult>> Handle(
        UnpublishQuizCommand request,
        CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz is null)
        {
            return RequestResponse<UnpublishQuizResult>.Fail(
                "Quiz not found.",
                StatusCodes.Status404NotFound);
        }

        if (quiz.Status != QuizStatus.Published)
        {
            return RequestResponse<UnpublishQuizResult>.Ok(
                new UnpublishQuizResult(quiz.Id, quiz.Status.ToString()),
                "Quiz is not currently published.");
        }

        quiz.Status = QuizStatus.Draft;
        quiz.PublishedAt = null;

        await _quizRepository.UpdateAsync(quiz);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<UnpublishQuizResult>.Ok(
            new UnpublishQuizResult(quiz.Id, quiz.Status.ToString()),
            "Quiz unpublished successfully.");
    }
}
