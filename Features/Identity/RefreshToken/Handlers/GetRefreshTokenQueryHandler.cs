using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.RefreshToken.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.RefreshToken.Handlers;

public class GetRefreshTokenQueryHandler : IRequestHandler<GetRefreshTokenQuery, Domain.Entities.Identity.RefreshToken?>
{
    private readonly IGenericRepository<Domain.Entities.Identity.RefreshToken> _refreshTokenRepo;

    public GetRefreshTokenQueryHandler(IGenericRepository<Domain.Entities.Identity.RefreshToken> refreshTokenRepo)
    {
        _refreshTokenRepo = refreshTokenRepo;
    }

    public async Task<Domain.Entities.Identity.RefreshToken?> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _refreshTokenRepo.Get(t => t.Token == request.Token)
                                      .Include(t => t.User)
                                          .ThenInclude(u => u.RefreshTokens)
                                      .FirstOrDefaultAsync(cancellationToken);
    }
}
