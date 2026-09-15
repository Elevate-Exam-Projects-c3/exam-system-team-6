using exam_system.Features.Diplomas.AdminDeleteDiploma.Orchestrators;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminDeleteDiploma.Controllers;

[ApiController]
[Route("api/admin/diplomas")]
//[Authorize(Roles = "Admin")]
public class DeleteDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public DeleteDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpDelete]
    public async Task<ActionResult<EndpointResponse>> DeleteDiploma(
        Guid diplomaId,
        CancellationToken cancellationToken)
    {
        var request = new DeleteDiplomaOrchestrator(diplomaId);

        var result = await _mediator.Send(
            request,
            cancellationToken);

        var response = EndpointResponse.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}