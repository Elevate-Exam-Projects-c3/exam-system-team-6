using exam_system.Features.Quizzes.AdminQuizPublishCheck.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Validators;

public class GetQuizQuestionsForPublishCheckQueryValidator
    : AbstractValidator<GetQuizQuestionsForPublishCheckQuery>
{
    public GetQuizQuestionsForPublishCheckQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
