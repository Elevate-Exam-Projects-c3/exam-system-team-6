using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.Login.Queries;

public record GetUserByEmailQuery : IRequest<ApplicationUser?>
{
    public string Email { get; set; } = string.Empty;
}
