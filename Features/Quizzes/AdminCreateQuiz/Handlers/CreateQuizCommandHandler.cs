using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Quizzes.AdminCreateQuiz.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;

namespace exam_system.Features.Quizzes.AdminCreateQuiz.Handlers;

public class CreateQuizCommandHandler(IGenericRepository<Quiz> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateQuizCommand, RequestResponse<Guid>>
{
    public async Task<RequestResponse<Guid>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = new Quiz { DiplomaId = request.CreateQuizDto.DiplomaId, 
            Title = request.CreateQuizDto.Title, 
            Instructions = request.CreateQuizDto.Instructions,
            DurationMinutes = request.CreateQuizDto.DurationMinutes, 
            PassScore = request.CreateQuizDto.PassScore ?? 60, 
            MaxAttempts = request.CreateQuizDto.MaxAttempts,
            StartDate = request.CreateQuizDto.StartDate,
            EndDate = request.CreateQuizDto.EndDate
        }; 
        
        await repository.AddAsync(quiz);
        await unitOfWork.SaveChangesAsync(cancellationToken);

      return RequestResponse<Guid>.Created(
          quiz.Id,
          "Quiz created successfully"
      );
    }
}