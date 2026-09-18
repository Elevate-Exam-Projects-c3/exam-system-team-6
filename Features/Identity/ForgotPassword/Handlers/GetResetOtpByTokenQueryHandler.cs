using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class GetResetOtpByTokenQueryHandler(IGenericRepository<PasswordResetOtp> otpRepo)
    : IRequestHandler<GetResetOtpByTokenQuery, GetResetOtpByTokenQueryResult?>
{
    public async Task<GetResetOtpByTokenQueryResult?> Handle(GetResetOtpByTokenQuery request, CancellationToken cancellationToken)
    {
        return await otpRepo.Get(o => o.ResetToken == request.ResetToken && !o.IsUsed)
                            .AsNoTracking()
                            .Select(o => new GetResetOtpByTokenQueryResult(
                                o.Id,
                                o.UserId,
                                o.ResetTokenExpiresAt
                            ))
                            .FirstOrDefaultAsync(cancellationToken);
    }
}
