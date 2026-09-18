using FluentValidation;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;

namespace exam_system.Features.Identity.ForgotPassword.Validators;

public class ForgotPasswordOrchestratorValidator : AbstractValidator<ForgotPasswordOrchestrator>
{
    public ForgotPasswordOrchestratorValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");
    }
}
