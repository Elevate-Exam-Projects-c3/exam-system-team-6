using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetQuestionForManagementQueryHandler(IGenericRepository<Question> questionRepository)
    : IRequestHandler<GetQuestionForManagementQuery, Question?>
{
    public async Task<Question?> Handle(GetQuestionForManagementQuery request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetByIdAsync(request.QuestionId);

        return question is not null && question.QuizId == request.QuizId
            ? question
            : null;
    }
}
