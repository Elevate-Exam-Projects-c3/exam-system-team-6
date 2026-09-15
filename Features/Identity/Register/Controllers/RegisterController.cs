using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Register.Orchestrators;
using exam_system.Features.Identity.Register.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/identity/register")]
public class RegisterController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterStudentViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var orchestrator = new RegisterStudentOrchestrator
        {
            FullName = viewModel.FullName,
            Email = viewModel.Email,
            Password = viewModel.Password
        };

        var result = await mediator.Send(orchestrator, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<RegisterResponse>.FromResult(result)
        );
    }
}
