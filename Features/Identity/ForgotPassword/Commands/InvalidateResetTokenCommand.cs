using MediatR;

namespace exam_system.Features.Identity.ForgotPassword.Commands;

public record InvalidateResetTokenCommand(Guid OtpId) : IRequest;
