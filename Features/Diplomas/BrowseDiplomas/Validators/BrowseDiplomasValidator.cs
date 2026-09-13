using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using FluentValidation;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Validators;

public class BrowseDiplomasValidator : AbstractValidator<BrowseDiplomasQuery>
{
    public BrowseDiplomasValidator()
    {
        RuleFor(x => x.PageIndex)
            .InclusiveBetween(1, 10000)
            .WithMessage("PageIndex must be between 1 and 10000.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}