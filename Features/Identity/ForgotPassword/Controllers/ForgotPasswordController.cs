using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Features.Identity.ForgotPassword.Orchestrators;
using exam_system.Features.Identity.ForgotPassword.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/identity/forgot-password")]
public class ForgotPasswordController(IMediator mediator) : ControllerBase
{
    [HttpPost("request-code")]
    [HttpPost("")]
    public async Task<IActionResult> RequestResetCode(
        [FromBody] ForgotPasswordViewModel viewModel,
        CancellationToken ct)
    {
        var orchestrator = new ForgotPasswordOrchestrator(viewModel.Email);
        var result = await mediator.Send(orchestrator, ct);
        var response = EndpointResponse.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("verify-code")]
    public async Task<IActionResult> VerifyResetCode(
        [FromBody] VerifyResetOtpViewModel viewModel,
        CancellationToken ct)
    {
        var orchestrator = new VerifyResetOtpOrchestrator(viewModel.Email, viewModel.Otp);
        var result = await mediator.Send(orchestrator, ct);
        var response = EndpointResponse<VerifyResetOtpResponse>.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordViewModel viewModel,
        CancellationToken ct)
    {
        var orchestrator = new ResetPasswordOrchestrator(
            viewModel.ResetToken,
            viewModel.NewPassword,
            viewModel.ConfirmPassword);

        var result = await mediator.Send(orchestrator, ct);
        var response = EndpointResponse.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }
}
