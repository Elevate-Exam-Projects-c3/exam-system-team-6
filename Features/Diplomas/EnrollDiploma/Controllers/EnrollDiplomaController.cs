using exam_system.Features.Diplomas.EnrollDiploma.Commands;
using exam_system.Features.Diplomas.EnrollDiploma.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.EnrollDiploma.Controllers;

[ApiController]
[Route("api/student/diplomas")]
//[Authorize(Roles = "Student")]
public class EnrollDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public EnrollDiplomaController(IMediator mediator)
        {
        _mediator = mediator;
        }
    
    [HttpPost("{id:guid}/enroll")]
    public async Task<ActionResult<EndpointResponse<Guid>>> Enroll(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new EnrollDiplomaOrchestrator(id);

        var result = await _mediator.Send(
            request,
            cancellationToken);

        var response =
            EndpointResponse<Guid>.FromResult(result);

        return StatusCode(
            response.StatusCode,
            response);
    }
}