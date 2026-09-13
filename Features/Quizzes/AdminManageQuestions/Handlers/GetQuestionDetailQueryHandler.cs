using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetQuestionDetailQueryHandler(IGenericRepository<Question> questionRepository)
    : IRequestHandler<GetQuestionDetailQuery, RequestResponse<QuestionResponse>>
{
    public async Task<RequestResponse<QuestionResponse>> Handle(GetQuestionDetailQuery request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetByIdAsync(request.QuestionId, q => q.Options);

        if (question is null || question.QuizId != request.QuizId)
        {
            return RequestResponse<QuestionResponse>.Fail(
                "Question not found",
                404);
        }

        return RequestResponse<QuestionResponse>.Ok(
            QuestionResponse.FromEntity(question),
            "Success"
        );
    }
}
