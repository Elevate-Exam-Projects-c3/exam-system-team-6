using MediatR;
using exam_system.Domain.Entities.Identity;

namespace exam_system.Features.Identity.Register.Commands;

public record CreateUserCommand(ApplicationUser User) : IRequest;
