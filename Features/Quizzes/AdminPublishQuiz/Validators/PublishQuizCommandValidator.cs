using exam_system.Features.Quizzes.AdminPublishQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Validators;

public class PublishQuizCommandValidator : AbstractValidator<PublishQuizCommand>
{
    public PublishQuizCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
