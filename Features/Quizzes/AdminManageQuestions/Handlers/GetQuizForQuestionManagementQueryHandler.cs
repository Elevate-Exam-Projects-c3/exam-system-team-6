using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class GetQuizForQuestionManagementQueryHandler(IGenericRepository<Quiz> quizRepository)
    : IRequestHandler<GetQuizForQuestionManagementQuery, Quiz?>
{
    public async Task<Quiz?> Handle(GetQuizForQuestionManagementQuery request, CancellationToken cancellationToken)
    {
        return await quizRepository.GetByIdAsync(request.QuizId);
    }
}
