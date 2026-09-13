using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Register.Commands;

public record RegisterStudentCommand(
    string FullName,
    string Email,
    string Password
) : IRequest<RequestResponse<RegisterResponse>>;
