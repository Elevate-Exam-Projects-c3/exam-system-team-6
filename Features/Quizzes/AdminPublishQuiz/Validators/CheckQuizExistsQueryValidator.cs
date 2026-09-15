using exam_system.Features.Quizzes.AdminPublishQuiz.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Validators;

public class CheckQuizExistsQueryValidator : AbstractValidator<CheckQuizExistsQuery>
{
    public CheckQuizExistsQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
