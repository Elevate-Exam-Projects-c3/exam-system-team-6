using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Register.Commands;

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
    public async Task<IActionResult> Register([FromBody] RegisterStudentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        if (!result.Success)
        {
            return StatusCode(result.StatusCode, result);
        }

        return StatusCode(result.StatusCode, result);
    }
}
