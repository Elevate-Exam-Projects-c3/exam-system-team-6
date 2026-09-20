using FluentValidation;
using exam_system.Features.Attempts.GetAttemptResults.Orchestrators;

namespace exam_system.Features.Attempts.GetAttemptResults.Validators;

public class ViewAttemptResultsOrchestratorValidator : AbstractValidator<ViewAttemptResultsOrchestrator>
{
    public ViewAttemptResultsOrchestratorValidator()
    {
        RuleFor(x => x.AttemptId)
            .NotEmpty()
            .WithMessage("Attempt ID is required.");

        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("Student ID is required.");
    }
}
