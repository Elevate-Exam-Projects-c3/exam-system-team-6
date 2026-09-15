using exam_system.Features.Quizzes.AdminUnpublishQuiz.Orchestrators;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminUnpublishQuiz.Validators;

public class UnpublishQuizOrchestratorValidator : AbstractValidator<UnpublishQuizOrchestrator>
{
    public UnpublishQuizOrchestratorValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
