using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.Register.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.Register.Handlers;

public class CheckEmailExistsQueryHandler : IRequestHandler<CheckEmailExistsQuery, bool>
{
    private readonly IGenericRepository<ApplicationUser> _userRepo;

    public CheckEmailExistsQueryHandler(IGenericRepository<ApplicationUser> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<bool> Handle(CheckEmailExistsQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        return await _userRepo.Get(u => u.Email == normalizedEmail)
                              .AnyAsync(cancellationToken);
    }
}
