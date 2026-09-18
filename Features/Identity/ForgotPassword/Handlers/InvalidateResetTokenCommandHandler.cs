using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class InvalidateResetTokenCommandHandler(
    IGenericRepository<PasswordResetOtp> otpRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<InvalidateResetTokenCommand>
{
    public async Task<Unit> Handle(InvalidateResetTokenCommand request, CancellationToken cancellationToken)
    {
        var otpRecord = await otpRepo.GetByIdAsync(request.OtpId);
        if (otpRecord != null)
        {
            otpRecord.IsUsed = true;
            otpRecord.ResetToken = null;
            otpRecord.ResetTokenExpiresAt = null;
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
