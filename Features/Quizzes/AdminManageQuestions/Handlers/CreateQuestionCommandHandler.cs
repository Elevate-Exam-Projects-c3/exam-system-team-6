using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Handlers;

public class CreateQuestionCommandHandler(
    IGenericRepository<Quiz> quizRepository,
    IGenericRepository<Question> questionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateQuestionCommand, RequestResponse<Guid>>
{
    public async Task<RequestResponse<Guid>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await quizRepository.GetByIdAsync(request.QuizId);

        if (quiz is null)
        {
            return RequestResponse<Guid>.Fail(
                "Quiz not found",
                404);
        }

        var question = new Question
        {
            QuizId = quiz.Id,
            Text = request.Text,
            Explanation = request.Explanation,
            OrderIndex = request.OrderIndex
        };

        foreach (var option in request.Options)
        {
            question.Options.Add(new QuestionOption
            {
                OptionText = option.OptionText,
                IsCorrect = option.IsCorrect
            });
        }

        await questionRepository.AddAsync(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return RequestResponse<Guid>.Created(
            question.Id,
            "Question created successfully"
        );
    }
}
