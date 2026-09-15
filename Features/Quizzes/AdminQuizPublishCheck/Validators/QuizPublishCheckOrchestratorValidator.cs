using exam_system.Features.Quizzes.AdminQuizPublishCheck.Orchestrators;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminQuizPublishCheck.Validators;

public class QuizPublishCheckOrchestratorValidator : AbstractValidator<QuizPublishCheckOrchestrator>
{
    public QuizPublishCheckOrchestratorValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
