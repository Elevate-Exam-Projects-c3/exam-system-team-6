using System.Text.Json.Serialization;

namespace exam_system.Features.Identity.Login;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    [JsonIgnore]
    public string? RefreshToken { get; set; }
}
