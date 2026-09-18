using MediatR;
using Microsoft.EntityFrameworkCore;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Identity.ForgotPassword.Queries;
using exam_system.Persistence.DataAccess;

namespace exam_system.Features.Identity.ForgotPassword.Handlers;

public class GetEmailExistQueryHandler : IRequestHandler<GetEmailExistQuery, GetEmailExistQueryResult?>
{
    private readonly IGenericRepository<ApplicationUser> _userRepo;

    public GetEmailExistQueryHandler(IGenericRepository<ApplicationUser> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<GetEmailExistQueryResult?> Handle(GetEmailExistQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        return await _userRepo.Get(u => u.Email == normalizedEmail)
                              .AsNoTracking()
                              .Select(u => new GetEmailExistQueryResult(
                                  u.Id,
                                  u.FullName,
                                  u.Email,
                                  u.PasswordResetOtps
                                   .OrderByDescending(o => o.CreatedAt)
                                   .Select(o => (DateTime?)o.CreatedAt)
                                   .FirstOrDefault()
                              ))
                              .FirstOrDefaultAsync(cancellationToken);
    }
}
