using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Features.Identity.Register.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Controllers;

[ApiController]
[Route("api/identity/register")]
public class RegisterController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterStudentViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var command = new RegisterStudentCommand
        {
            FullName = viewModel.FullName,
            Email = viewModel.Email,
            Password = viewModel.Password
        };

        RegisterStudentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<RegisterResponse>.FromResult(result)
        );
    }
}
