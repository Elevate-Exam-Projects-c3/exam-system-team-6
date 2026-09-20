using exam_system.Domain.Entities.Attempts;
using exam_system.Features.Attempts.SubmitQuestionAnswer.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Attempts.SubmitQuestionAnswer.Handlers;

public class SaveQuestionAnswerHandler( IGenericRepository<StudentQuestionAnswer> answerRepository) :IRequestHandler<SaveQuestionAnswerCommand, RequestResponse>
{
    public async Task<RequestResponse> Handle(SaveQuestionAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await answerRepository
            .Get(x =>
                x.AttemptId == request.AttemptId &&
                x.QuestionId == request.QuestionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (answer is null)
        {
            answer = new StudentQuestionAnswer
            {
                AttemptId = request.AttemptId,
                QuestionId = request.QuestionId,
                SelectedOptionId = request.SelectedOptionId,
                AnsweredAt = DateTime.UtcNow
            };

            await answerRepository.AddAsync(answer);
        }
        else
        {
            answer.SelectedOptionId = request.SelectedOptionId;
            answer.AnsweredAt = DateTime.UtcNow;
        }
        
        return RequestResponse.Ok(); 
    }
}