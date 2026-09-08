using exam_system.Features.Quizzes.AdminUpdateQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminUpdateQuiz.Validators;

public class UpdateQuizCommandValidator : AbstractValidator<UpdateQuizCommand>
{
    public UpdateQuizCommandValidator()
    {
        RuleFor(x=>x.Id)
            .NotEmpty().WithMessage("Quiz ID is required.");
        
        RuleFor(x => x.UpdateQuizDto.Title)
            .Length(3, 200).When(x => x.UpdateQuizDto.Title != null)
            .WithMessage("Title must be between 3 and 200 characters.");
        
        RuleFor(x => x.UpdateQuizDto.DurationMinutes)
            .GreaterThan(0).When(x => x.UpdateQuizDto.DurationMinutes.HasValue)
            .WithMessage("Duration must be greater than 0 minutes.");

        RuleFor(x => x.UpdateQuizDto.PassScore)
            .InclusiveBetween(0, 100).When(x => x.UpdateQuizDto.PassScore.HasValue)
            .WithMessage("Pass score must be between 0 and 100.");

        RuleFor(x => x.UpdateQuizDto.MaxAttempts)
            .GreaterThan(0).When(x => x.UpdateQuizDto.MaxAttempts.HasValue)
            .WithMessage("Max attempts must be greater than 0.");
    }
}