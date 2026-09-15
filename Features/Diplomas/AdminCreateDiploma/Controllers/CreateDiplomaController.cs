using exam_system.Features.Diplomas.AdminCreateDiploma.Commands;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.AdminCreateDiploma.Controllers;

[ApiController]
[Route("api/admin/diplomas")]
//[Authorize(Roles = "Admin")]
public class CreateDiplomaController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreateDiplomaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<EndpointResponse<Guid>>> CreateDiploma(
        [FromBody] CreateDiplomaCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        var response = EndpointResponse<Guid>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}