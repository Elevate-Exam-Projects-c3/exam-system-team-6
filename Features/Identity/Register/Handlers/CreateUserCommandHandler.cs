using MediatR;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Commands;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Register.Handlers;

public class CreateUserCommandHandler(
    IGenericRepository<ApplicationUser> userRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await userRepo.AddAsync(request.User);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
