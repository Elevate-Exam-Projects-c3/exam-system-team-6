using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Handlers;

public class PublishQuizCommandHandler
    : IRequestHandler<PublishQuizCommand, RequestResponse<PublishQuizResult>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishQuizCommandHandler(
        IGenericRepository<Quiz> quizRepository,
        IUnitOfWork unitOfWork)
    {
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse<PublishQuizResult>> Handle(
        PublishQuizCommand request,
        CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz is null)
        {
            return RequestResponse<PublishQuizResult>.Fail(
                "Quiz not found.",
                StatusCodes.Status404NotFound);
        }

        if (quiz.Status == QuizStatus.Published)
        {
            return RequestResponse<PublishQuizResult>.Ok(
                new PublishQuizResult(quiz.Id, quiz.Status.ToString(), quiz.PublishedAt),
                "Quiz is already published.");
        }

        quiz.Status = QuizStatus.Published;
        quiz.PublishedAt = DateTime.UtcNow;

        await _quizRepository.UpdateAsync(quiz);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<PublishQuizResult>.Ok(
            new PublishQuizResult(quiz.Id, quiz.Status.ToString(), quiz.PublishedAt),
            "Quiz published successfully.");
    }
}
