using MediatR;
using exam_system.Features.Identity.RefreshToken.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, Domain.Entities.Identity.RefreshToken>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepo;

    public AddRefreshTokenCommandHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepo)
    {
        _refreshTokenRepo = refreshTokenRepo;
    }

    public async Task<Domain.Entities.Identity.RefreshToken> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = new Domain.Entities.Identity.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Token = request.Token,
            ExpiresAt = request.ExpiresAt,
            IsUsed = false,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepo.AddAsync(refreshTokenEntity);

        return refreshTokenEntity;
    }
}
