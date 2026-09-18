using FluentValidation;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;

namespace exam_system.Features.Identity.ForgotPassword.Validators;

public class VerifyResetOtpOrchestratorValidator : AbstractValidator<VerifyResetOtpOrchestrator>
{
    public VerifyResetOtpOrchestratorValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("Verification code is required.")
            .Matches(@"^[0-9]{6}$").WithMessage("Verification code must be exactly 6 numeric digits.");
    }
}
