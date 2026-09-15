using System.Text.Json.Serialization;

namespace exam_system.Features.Identity.RefreshToken;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }
}
