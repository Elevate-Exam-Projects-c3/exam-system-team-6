using exam_system.Features.Diplomas.AdminDeleteDiploma.Commands;
using FluentValidation;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Validators;

public class DeleteDiplomaValidator : AbstractValidator<DeleteDiplomaCommand>
{
    public DeleteDiplomaValidator()
    {
        RuleFor(x => x.DiplomaId)
            .NotEmpty()
            .WithMessage("DiplomaId is required.");
    }
}