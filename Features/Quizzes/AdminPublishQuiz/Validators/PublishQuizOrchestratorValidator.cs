using exam_system.Features.Quizzes.AdminPublishQuiz.Orchestrators;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminPublishQuiz.Validators;

public class PublishQuizOrchestratorValidator : AbstractValidator<PublishQuizOrchestrator>
{
    public PublishQuizOrchestratorValidator()
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("QuizId is required.");
    }
}
