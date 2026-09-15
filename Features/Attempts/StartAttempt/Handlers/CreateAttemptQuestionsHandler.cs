using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Builders;
using exam_system.Features.Attempts.StartAttempt.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class CreateAttemptQuestionsHandler(IGenericRepository<AttemptQuestion> attemptQuestionRepository, AttemptQuestionBuilder questionBuilder):
    IRequestHandler<CreateAttemptQuestionsCommand, RequestResponse<bool>>
{
    public async Task<RequestResponse<bool>> Handle(CreateAttemptQuestionsCommand request, CancellationToken cancellationToken)
    {
        var attemptQuestions = questionBuilder.Build(
            request.AttemptId,
            request.QuestionIds);

        await attemptQuestionRepository.AddRangeAsync(attemptQuestions);

        return RequestResponse<bool>.Ok(true);
    }
}