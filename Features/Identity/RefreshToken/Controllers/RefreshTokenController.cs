using MediatR;
using Microsoft.AspNetCore.Mvc;
using exam_system.Features.Identity.RefreshToken.Orchestrators;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.RefreshToken.Controllers;

[ApiController]
[Route("api/identity/refresh-token")]
public class RefreshTokenController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await mediator.Send(new RefreshTokenOrchestrator(refreshToken), ct);

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
        else
        {
            // Clear compromised or invalid cookie client-side
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }

        var response = EndpointResponse<RefreshTokenResponse>.FromResult(result);
        return StatusCode(response.StatusCode, response);
    }
}
