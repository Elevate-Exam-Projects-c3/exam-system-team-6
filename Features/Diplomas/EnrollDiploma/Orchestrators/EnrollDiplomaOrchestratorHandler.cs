using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Quizzes;
using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;

public class EnrollDiplomaOrchestratorHandler
    : IRequestHandler<
        EnrollDiplomaOrchestrator,
        RequestResponse<Guid>>
{
    private readonly IMediator _mediator;

    public EnrollDiplomaOrchestratorHandler(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<RequestResponse<Guid>> Handle(
        EnrollDiplomaOrchestrator request,
        CancellationToken cancellationToken)
    {
        var diplomaResult = await _mediator.Send(
            new GetDiplomaForEnrollmentQuery(
                request.DiplomaId),
            cancellationToken);

        if (!diplomaResult.Success)
        {
            return RequestResponse<Guid>.Fail(
                diplomaResult.Message,
                diplomaResult.StatusCode);
        }

        var quizResult = await _mediator.Send(
            new HasPublishedQuizQuery(
                request.DiplomaId),
            cancellationToken);

        if (!quizResult.Success)
        {
            return RequestResponse<Guid>.Fail(
                quizResult.Message,
                quizResult.StatusCode);
        }

        if (quizResult.Data?.HasPublishedQuiz != true)
        {
            return RequestResponse<Guid>.Fail(
                "Cannot enroll in a diploma without a published quiz.",
                StatusCodes.Status409Conflict);
        }

        var enrollmentResult = await _mediator.Send(
            new HasActiveEnrollmentQuery(
                request.DiplomaId),
            cancellationToken);

        if (!enrollmentResult.Success)
        {
            return RequestResponse<Guid>.Fail(
                enrollmentResult.Message,
                enrollmentResult.StatusCode);
        }

        if (enrollmentResult.Data?.HasActiveEnrollment == true)
        {
            return RequestResponse<Guid>.Fail(
                "Student is already enrolled in this diploma.",
                StatusCodes.Status409Conflict);
        }

        return await _mediator.Send(
            new EnrollDiplomaCommand(
                request.DiplomaId),
            cancellationToken);
    }
}