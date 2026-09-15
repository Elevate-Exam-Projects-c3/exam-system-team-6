using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using FluentValidation;

namespace exam_system.Features.Diplomas.EnrollDiploma.Validators;

public class EnrollDiplomaValidator : AbstractValidator<EnrollDiplomaCommand>
{
    public EnrollDiplomaValidator()
    {
        RuleFor(diplomaId => diplomaId.DiplomaId)
            .NotEmpty()
            .WithMessage("DiplomaId is required");
    }
}