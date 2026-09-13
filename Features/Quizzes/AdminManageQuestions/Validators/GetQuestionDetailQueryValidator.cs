using exam_system.Features.Quizzes.AdminManageQuestions.Queries;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class GetQuestionDetailQueryValidator : AbstractValidator<GetQuestionDetailQuery>
{
    public GetQuestionDetailQueryValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required.");

        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("Question ID is required.");
    }
}
