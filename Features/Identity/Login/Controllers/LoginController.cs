using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using exam_system.Features.Identity.Login.Orchestrators;
using exam_system.Features.Identity.Login.ViewModels;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Login.Controllers;

[ApiController]
[Route("api/identity/login")]
[EnableRateLimiting("identity")]
public class LoginController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel, CancellationToken ct)
    {
        var orchestrator = new LoginOrchestrator
        {
            Email = viewModel.Email,
            Password = viewModel.Password
        };

        var result = await mediator.Send(orchestrator, ct);

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
