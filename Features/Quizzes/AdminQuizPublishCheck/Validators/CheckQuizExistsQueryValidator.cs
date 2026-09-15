using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Validators;

public class CheckQuizExistsQueryValidator : AbstractValidator<CheckQuizExistsQuery>
{
    public CheckQuizExistsQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
