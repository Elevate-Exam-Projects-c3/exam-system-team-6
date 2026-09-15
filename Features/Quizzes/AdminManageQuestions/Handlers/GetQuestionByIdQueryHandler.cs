using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetQuestionByIdQueryHandler
    : IRequestHandler<GetQuestionByIdQuery, RequestResponse<QuestionData>>
{
    private readonly IGenericRepository<Question> _questionRepository;

    public GetQuestionByIdQueryHandler(
        IGenericRepository<Question> questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<RequestResponse<QuestionData>> Handle(
        GetQuestionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var question = await _questionRepository
            .Get(q => q.Id == request.QuestionId && !q.IsDeleted)
            .Select(q => new QuestionData(q.Id, q.QuizId, q.Text))
            .FirstOrDefaultAsync(cancellationToken);

        if (question is null)
        {
            return RequestResponse<QuestionData>.Fail(
                "Question not found.",
                StatusCodes.Status404NotFound);
        }

        return RequestResponse<QuestionData>.Ok(question);
    }
}
