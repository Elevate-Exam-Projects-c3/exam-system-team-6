using MediatR;

namespace exam_system.Features.Identity.Login.Commands;

public record GenerateTokensCommand : IRequest<GenerateTokensResult>
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}

public record GenerateTokensResult(string AccessToken, string RefreshToken);
