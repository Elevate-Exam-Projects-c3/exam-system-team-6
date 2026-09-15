using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.Logout.Commands;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Logout.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/identity/logout")]
public class LogoutController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await mediator.Send(new LogoutCommand(refreshToken), ct);

        // Clear the cookie client-side
        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        var response = EndpointResponse.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }
}
