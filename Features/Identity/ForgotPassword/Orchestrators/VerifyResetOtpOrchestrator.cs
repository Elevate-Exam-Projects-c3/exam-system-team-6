using MediatR;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.ForgotPassword.Orchestrators;

public record VerifyResetOtpOrchestrator(string Email, string Otp) : IRequest<RequestResponse<VerifyResetOtpResponse>>;
