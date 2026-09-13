using exam_system.Features.Quizzes.AdminManageQuestions.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty().WithMessage("Quiz ID is required.");

        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("Question ID is required.");
    }
}
