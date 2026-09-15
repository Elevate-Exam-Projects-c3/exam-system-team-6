using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetQuizByIdQueryHandler
    : IRequestHandler<GetQuizByIdQuery, RequestResponse<QuizData>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public GetQuizByIdQueryHandler(
        IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse<QuizData>> Handle(
        GetQuizByIdQuery request,
        CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository
            .Get(q => q.Id == request.QuizId && !q.IsDeleted)
            .Select(q => new QuizData(q.Id, q.Status))
            .FirstOrDefaultAsync(cancellationToken);

        if (quiz is null)
        {
            return RequestResponse<QuizData>.Fail(
                "Quiz not found.",
                StatusCodes.Status404NotFound);
        }

        return RequestResponse<QuizData>.Ok(quiz);
    }
}
