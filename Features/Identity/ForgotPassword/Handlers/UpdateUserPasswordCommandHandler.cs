using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class UpdateUserPasswordCommandHandler(
    IGenericRepository<ApplicationUser> userRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserPasswordCommand>
{
    public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByIdAsync(request.UserId);
        if (user != null)
        {
            user.PasswordHash = request.NewPasswordHash;
            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
