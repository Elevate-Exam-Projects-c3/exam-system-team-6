using exam_system.Features.Quizzes.AdminUnpublishQuiz.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Validators;

public class CheckQuizExistsQueryValidator : AbstractValidator<CheckQuizExistsQuery>
{
    public CheckQuizExistsQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
