using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using FluentValidation;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Validators;

public class CreateDiplomaCommandValidator : AbstractValidator<CreateDiplomaCommand>
{
    public CreateDiplomaCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Length(3, 200)
            .WithMessage("Title must be between 3 and 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");
    }
}