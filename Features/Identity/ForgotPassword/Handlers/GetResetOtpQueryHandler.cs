using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class GetResetOtpQueryHandler(IGenericRepository<PasswordResetOtp> otpRepo)
    : IRequestHandler<GetResetOtpQuery, PasswordResetOtp?>
{
    public async Task<PasswordResetOtp?> Handle(GetResetOtpQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        return await otpRepo.Get(o => o.Email == normalizedEmail && !o.IsUsed)
                            .OrderByDescending(o => o.CreatedAt)
                            .FirstOrDefaultAsync(cancellationToken);
    }
}
