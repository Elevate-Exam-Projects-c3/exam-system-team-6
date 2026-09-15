using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Login.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Login.Handlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, ApplicationUser?>
{
    private readonly IGenericRepository<ApplicationUser> _userRepo;

    public GetUserByEmailQueryHandler(IGenericRepository<ApplicationUser> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<ApplicationUser?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        return await _userRepo.Get(u => u.Email.ToLower() == normalizedEmail)
                              .FirstOrDefaultAsync(cancellationToken);
    }
}
