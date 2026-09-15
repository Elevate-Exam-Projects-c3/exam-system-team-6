using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Common.Enums;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class ActivateUserAccountCommandHandler : IRequestHandler<ActivateUserAccountCommand, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepo;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUserAccountCommandHandler(IGenericRepository<ApplicationUser> userRepo, IUnitOfWork unitOfWork)
    {
        _userRepo = userRepo;
        _unitOfWork = unitOfWork;
    }


    public async Task<bool> Handle(ActivateUserAccountCommand request, CancellationToken cancellationToken)
    {
        var data = await _userRepo.Get(u => u.Id == request.UserId)
                                  .Select(u => new
                                  {
                                      User = u,
                                      Otp = u.EmailVerificationOtps.FirstOrDefault(o => o.Id == request.OtpId)
                                  })
                                  .FirstOrDefaultAsync(cancellationToken);

        if (data?.User == null || data.Otp == null)
        {
            return false;
        }

        data.Otp.IsUsed = true;
        data.User.EmailConfirmed = true;
        data.User.AccountStatus = AccountStatus.Active;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
