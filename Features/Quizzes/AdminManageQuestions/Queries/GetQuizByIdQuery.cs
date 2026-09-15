using exam_system.Common.Enums;
using exam_system.Features.Shared;
using MediatR;

namespace exam_system.Features.Quizzes.AdminManageQuestions.Queries;

public record GetQuizByIdQuery(
    Guid QuizId
) : IRequest<RequestResponse<QuizData>>;

public record QuizData(
    Guid Id,
    QuizStatus Status
);
