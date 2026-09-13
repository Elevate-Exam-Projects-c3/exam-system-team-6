using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class GetQuizQuestionsQueryValidator : AbstractValidator<GetQuizQuestionsQuery>
{
    public GetQuizQuestionsQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required.");
    }
}
