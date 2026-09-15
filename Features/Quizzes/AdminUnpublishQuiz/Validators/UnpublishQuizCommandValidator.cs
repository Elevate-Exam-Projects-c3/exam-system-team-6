using exam_system.Features.Quizzes.AdminUnpublishQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Validators;

public class UnpublishQuizCommandValidator : AbstractValidator<UnpublishQuizCommand>
{
    public UnpublishQuizCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
