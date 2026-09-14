using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.GetQuiz.Dtos;
using exam_system.Features.Quizzes.GetQuiz.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.GetQuiz.Handlers;

public class GetQuizQueryHandler(IGenericRepository<Quiz> repository) : IRequestHandler<GetQuizQueryRequest, RequestResponse<GetQuizQueryDto>>
{
    public async Task<RequestResponse<GetQuizQueryDto>> Handle(GetQuizQueryRequest request, CancellationToken cancellationToken)
    {
        var quiz = await repository.GetByIdAsync(request.QuizId);

        if (quiz is null)
        {
            return RequestResponse<GetQuizQueryDto>.Fail(
                "Quiz not found",
                404);   
        }

        var dto = new GetQuizQueryDto
        {
            Id = quiz.Id,
            DiplomaId = quiz.DiplomaId,
            Title = quiz.Title,
            Instructions = quiz.Instructions,
            DurationMinutes = quiz.DurationMinutes,
            PassScore = quiz.PassScore,
            MaxAttempts = quiz.MaxAttempts,
            Status = quiz.Status.ToString(),
            PublishedAt = quiz.PublishedAt,
            StartDate = quiz.StartDate,
            EndDate = quiz.EndDate,
            CreatedAt = quiz.CreatedAt,
            UpdatedAt = quiz.UpdatedAt
        };

        return RequestResponse<GetQuizQueryDto>.Ok(dto, "Quiz retrieved successfully");
    }
}
