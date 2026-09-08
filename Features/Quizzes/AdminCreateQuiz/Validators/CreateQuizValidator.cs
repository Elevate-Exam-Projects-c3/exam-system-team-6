using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Validators;

public class CreateQuizValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizValidator()
    {
        RuleFor(x => x.CreateQuizDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 200).WithMessage("Title must be between 3 and 200 characters.");
        RuleFor(x => x.CreateQuizDto.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes.");

        RuleFor(x => x.CreateQuizDto.PassScore)
            .InclusiveBetween(0, 100).When(x => x.CreateQuizDto.PassScore.HasValue)
            .WithMessage("Pass score must be between 0 and 100.");

        RuleFor(x => x.CreateQuizDto.MaxAttempts)
            .GreaterThan(0).When(x => x.CreateQuizDto.MaxAttempts.HasValue)
            .WithMessage("Max attempts must be greater than 0.");
    }
}