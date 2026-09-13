using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;
using exam_system.Features.Identity.VerifyEmailOtp.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Controllers;

[ApiController]
[Route("api/identity/verify-email-otp")]
public class VerifyEmailOtpController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Verify(
        [FromBody] VerifyEmailOtpViewModel viewModel,
        CancellationToken cancellationToken)
    {
        var orchestrator = new VerifyEmailOtpOrchestrator
        {
            Email = viewModel.Email,
            Otp = viewModel.Otp
        };

        var result = await mediator.Send(orchestrator, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<VerifyEmailOtpResponse>.FromResult(result)
        );
    }
}
