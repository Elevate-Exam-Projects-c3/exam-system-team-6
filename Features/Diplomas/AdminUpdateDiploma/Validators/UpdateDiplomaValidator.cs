using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Dtos;
using FluentValidation;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Validators;

public class UpdateDiplomaValidator : AbstractValidator<UpdateDiplomaCommand>
{
    public UpdateDiplomaValidator()
    {
        RuleFor(x => x.Title)
            .Length(3, 200)
            .WithMessage("Title must be between 3 and 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");
            
    }
}