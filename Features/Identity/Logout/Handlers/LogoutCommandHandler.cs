using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Features.Identity.Logout.Commands;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;

using RefreshTokenEntity = exam_system.Domain.Entities.Identity.RefreshToken;

namespace exam_system.Features.Identity.Logout.Handlers;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, RequestResponse>
{
    private readonly IGenericRepository<RefreshTokenEntity> _refreshTokenRepo;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(
        IGenericRepository<RefreshTokenEntity> refreshTokenRepo,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepo = refreshTokenRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            var tokenEntity = await _refreshTokenRepo.Get(t => t.Token == request.RefreshToken)
                                                     .FirstOrDefaultAsync(cancellationToken);

            if (tokenEntity != null && !tokenEntity.IsRevoked)
            {
                tokenEntity.IsRevoked = true;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        return RequestResponse.Ok("Logged out successfully.");
    }
}
