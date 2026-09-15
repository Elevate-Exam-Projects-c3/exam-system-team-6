using exam_system.Features.Quizzes.AdminPublishQuiz.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Validators;

public class GetQuizQuestionsForPublishCheckQueryValidator
    : AbstractValidator<GetQuizQuestionsForPublishCheckQuery>
{
    public GetQuizQuestionsForPublishCheckQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
