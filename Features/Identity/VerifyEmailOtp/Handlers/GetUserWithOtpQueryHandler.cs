using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class GetUserWithOtpQueryHandler : IRequestHandler<GetUserWithOtpQuery, GetUserWithOtpQueryResult>
{
    private readonly IGenericRepository<ApplicationUser> _userRepo;

    public GetUserWithOtpQueryHandler(IGenericRepository<ApplicationUser> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<GetUserWithOtpQueryResult> Handle(GetUserWithOtpQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _userRepo.Get(u => u.Email.ToLower() == normalizedEmail)
                                  .Include(u => u.EmailVerificationOtps)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return new GetUserWithOtpQueryResult
            {
                User = null,
                LatestUnusedOtp = null
            };
        }

        var latestOtp = user.EmailVerificationOtps
            .Where(o => !o.IsUsed)
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefault();

        return new GetUserWithOtpQueryResult
        {
            User = user,
            LatestUnusedOtp = latestOtp
        };
    }
}
