using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class CreateQuestionOrchestratorValidator
    : AbstractValidator<CreateQuestionOrchestrator>
{
    public CreateQuestionOrchestratorValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");

        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Question text is required.")
            .MaximumLength(2000)
            .WithMessage("Question text must not exceed 2000 characters.");

        RuleFor(x => x.Explanation)
            .MaximumLength(4000)
            .When(x => !string.IsNullOrWhiteSpace(x.Explanation))
            .WithMessage("Explanation must not exceed 4000 characters.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("OrderIndex must be 0 or greater.");

        RuleFor(x => x.Options)
            .NotNull()
            .WithMessage("At least 2 options are required.")
            .Must(o => o.Count >= 2)
            .WithMessage("At least 2 options are required.")
            .Must(o => o.Count(opt => opt.IsCorrect) == 1)
            .WithMessage("Exactly one option must be marked as correct (IsCorrect = true).");

        RuleForEach(x => x.Options)
            .ChildRules(option =>
            {
                option.RuleFor(o => o.OptionText)
                    .NotEmpty()
                    .WithMessage("Option text is required.")
                    .MaximumLength(500)
                    .WithMessage("Option text must not exceed 500 characters.");
            });
    }
}
