using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public record ResetPasswordOrchestrator(
    string ResetToken,
    string NewPassword,
    string ConfirmPassword) : IRequest<RequestResponse>;
