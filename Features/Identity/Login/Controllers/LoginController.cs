using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Login.Controllers;

[ApiController]
[Route("api/identity/login")]
[EnableRateLimiting("identity")]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EndpointResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EndpointResponse<LoginResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(EndpointResponse<LoginResponse>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(EndpointResponse<LoginResponse>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);

        if (result.Success && !string.IsNullOrEmpty(result.Data?.RefreshToken))
        {
            Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        var response = EndpointResponse<LoginResponse>.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }
}
