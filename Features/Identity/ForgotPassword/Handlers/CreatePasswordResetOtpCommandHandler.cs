using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class CreatePasswordResetOtpCommandHandler : IRequestHandler<CreatePasswordResetOtpCommand>
{
    private readonly IGenericRepository<PasswordResetOtp> _otpRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePasswordResetOtpCommandHandler(
        IGenericRepository<PasswordResetOtp> otpRepo,
        IUnitOfWork unitOfWork)
    {
        _otpRepo = otpRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreatePasswordResetOtpCommand request, CancellationToken cancellationToken)
    {
        // Invalidate any previously active unused OTPs for this user
        var activeOtps = await _otpRepo.Get(o => o.UserId == request.OtpEntity.UserId && !o.IsUsed)
                                       .ToListAsync(cancellationToken);

        foreach (var oldOtp in activeOtps)
        {
            oldOtp.IsUsed = true;
        }

        await _otpRepo.AddAsync(request.OtpEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
