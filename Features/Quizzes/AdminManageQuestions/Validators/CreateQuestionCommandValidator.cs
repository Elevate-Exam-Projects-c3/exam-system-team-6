using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.");

        // Explanation is optional — only bounded to the DB column size.
        RuleFor(x => x.Explanation)
            .MaximumLength(1000).WithMessage("Explanation must not exceed 1000 characters.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Order index must be 0 or greater.");

        RuleFor(x => x.Options)
            .NotNull().WithMessage("At least 2 options are required.")
            .Must(options => options is not null && options.Count >= 2)
                .WithMessage("A question must have at least 2 options.");

        RuleFor(x => x.Options)
            .Must(options => options is not null && options.Count(o => o.IsCorrect) == 1)
                .WithMessage("Exactly one option must be marked as correct (IsCorrect = true).");

        RuleForEach(x => x.Options)
            .ChildRules(option =>
            {
                option.RuleFor(o => o.OptionText)
                    .NotEmpty().WithMessage("Option text is required.")
                    .MaximumLength(500).WithMessage("Option text must not exceed 500 characters.");
            });
    }
}
