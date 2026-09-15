using exam_system.Features.Quizzes.AdminManageQuestions.Orchestrators;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Validators;

public class DeleteQuestionOrchestratorValidator
    : AbstractValidator<DeleteQuestionOrchestrator>
{
    public DeleteQuestionOrchestratorValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");

        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("QuestionId is required.");
    }
}
