using MediatR;
using Microsoft.Extensions.Configuration;
using exam_system.Common.Services;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Login.Handlers;

public class GenerateTokensCommandHandler : IRequestHandler<GenerateTokensCommand, GenerateTokensResult>
{
    private readonly IJwtService _jwtService;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepo;
    private readonly IConfiguration _configuration;

    public GenerateTokensCommandHandler(
        IJwtService jwtService,
        IGenericRepository<RefreshToken> refreshTokenRepo,
        IConfiguration configuration)
    {
        _jwtService = jwtService;
        _refreshTokenRepo = refreshTokenRepo;
        _configuration = configuration;
    }

    public async Task<GenerateTokensResult> Handle(GenerateTokensCommand request, CancellationToken cancellationToken)
    {
        var accessToken = _jwtService.GenerateAccessToken(request.UserId, request.Email, request.Role);
        var refreshTokenString = _jwtService.GenerateRefreshToken();

        var expiryDaysStr = _configuration["JwtSettings:RefreshTokenExpiryDays"];
        var expiryDays = int.TryParse(expiryDaysStr, out var parsedDays) ? parsedDays : 7;

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
            IsUsed = false,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepo.AddAsync(refreshTokenEntity);

        return new GenerateTokensResult(accessToken, refreshTokenString);
    }
}
