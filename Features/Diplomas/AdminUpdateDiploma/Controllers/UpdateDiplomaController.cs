using exam_system.Features.Diplomas.AdminUpdateDiploma.Commands;
using exam_system.Features.Diplomas.AdminUpdateDiploma.Dtos;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminUpdateDiploma.Controllers;

[ApiController]
[Route("api/admin/diplomas")]
//[Authorize(Roles = "Admin")]
public class UpdateDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;

    public UpdateDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EndpointResponse<Guid>>> UpdateDiploma(
        Guid id,
        [FromBody] UpdateDiplomaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDiplomaCommand(
            id,
            request.Title,
            request.Description);

        var result = await _mediator.Send(command, cancellationToken);

        var response = EndpointResponse<Guid>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}