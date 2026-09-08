using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Controllers;

[ApiController]
[Route("api/identity/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegisterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EndpointResponse<RegisterResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(EndpointResponse<RegisterResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EndpointResponse<RegisterResponse>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterStudentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        var response = EndpointResponse<RegisterResponse>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}
