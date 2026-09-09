using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Controllers;

[ApiController]
[Route("api/identity/verify-email-otp")]
public class VerifyEmailOtpController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Verify(
        VerifyEmailOtpCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return StatusCode(
            result.StatusCode,
            EndpointResponse<VerifyEmailOtpResponse>.FromResult(result)
        );
    }
}
