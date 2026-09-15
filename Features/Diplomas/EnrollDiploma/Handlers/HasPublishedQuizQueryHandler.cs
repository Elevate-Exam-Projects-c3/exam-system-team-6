using exam_system.Common.Enums;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.EnrollDiploma.Dtos;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Handlers;

public class HasPublishedQuizQueryHandler
    : IRequestHandler<
        HasPublishedQuizQuery,
        RequestResponse<HasPublishedQuizDto>>
{
    private readonly IGenericRepository<Quiz> _quizRepository;

    public HasPublishedQuizQueryHandler(
        IGenericRepository<Quiz> quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<RequestResponse<HasPublishedQuizDto>> Handle(
        HasPublishedQuizQuery request,
        CancellationToken cancellationToken)
    {
        var hasPublishedQuiz = await _quizRepository
            .Get(q =>
                q.DiplomaId == request.DiplomaId &&
                q.Status == QuizStatus.Published &&
                !q.IsDeleted)
            .AnyAsync(cancellationToken);

        return RequestResponse<HasPublishedQuizDto>.Ok(
            new HasPublishedQuizDto
            {
                HasPublishedQuiz = hasPublishedQuiz
            });
    }
}