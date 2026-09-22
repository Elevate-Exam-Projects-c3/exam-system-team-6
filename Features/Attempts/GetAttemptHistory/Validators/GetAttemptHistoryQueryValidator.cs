using exam_system.Features.Attempts.GetAttemptHistory.Queries;
using FluentValidation;

namespace exam_system.Features.Attempts.GetAttemptHistory.Validators;

public class GetAttemptHistoryQueryValidator : AbstractValidator<GetAttemptHistoryQuery>
{
    public GetAttemptHistoryQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageIndex must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("StudentId is required.");
    }
}
