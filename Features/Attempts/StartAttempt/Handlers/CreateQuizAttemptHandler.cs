using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.StartAttempt.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Attempts.StartAttempt.Handlers;

public class CreateQuizAttemptHandler(IGenericRepository<QuizAttempt> attemptRepository, IUserContext userContext):IRequestHandler<CreateQuizAttemptCommand, RequestResponse<Guid>>
{
    public async Task<RequestResponse<Guid>> Handle(CreateQuizAttemptCommand request,
        CancellationToken cancellationToken)
    {
        // var studentId = userContext.UserId;

        var quizAttempt = new QuizAttempt
        {
            QuizId = request.QuizId,
            StudentId = request.StudentId,
        };

        await attemptRepository.AddAsync(quizAttempt);
        return RequestResponse<Guid>.Ok(quizAttempt.Id, "Quiz attempt created successfully.");
    }
}