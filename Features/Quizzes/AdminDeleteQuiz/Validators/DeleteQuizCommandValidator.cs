using exam_system.Features.Quizzes.AdminDeleteQuiz.Commands;
using FluentValidation;

namespace exam_system.Features.Quizzes.AdminDeleteQuiz.Validators;

public class DeleteQuizCommandValidator :AbstractValidator<DeleteQuizCommand>
{
    public DeleteQuizCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}