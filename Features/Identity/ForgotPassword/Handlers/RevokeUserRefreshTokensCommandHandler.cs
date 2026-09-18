using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

using RefreshTokenEntity = exam_system.Domain.Entities.Identity.RefreshToken;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class RevokeUserRefreshTokensCommandHandler(
    IGenericRepository<RefreshTokenEntity> refreshTokenRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<RevokeUserRefreshTokensCommand>
{
    public async Task<Unit> Handle(RevokeUserRefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var activeTokens = await refreshTokenRepo.Get(t => t.UserId == request.UserId && !t.IsRevoked)
                                                 .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.IsRevoked = true;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
