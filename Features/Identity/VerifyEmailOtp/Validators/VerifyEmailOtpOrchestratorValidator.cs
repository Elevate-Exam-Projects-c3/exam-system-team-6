using FluentValidation;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;

namespace exam_system.Features.Identity.VerifyEmailOtp.Validators;

public class VerifyEmailOtpOrchestratorValidator : AbstractValidator<VerifyEmailOtpOrchestrator>
{
    public VerifyEmailOtpOrchestratorValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(256);

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required.")
            .Length(6).WithMessage("OTP must be 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("OTP must contain only digits.");
    }
}
