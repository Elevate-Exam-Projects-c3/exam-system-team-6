using MediatR;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class VerifyEmailOtpCommandHandler : IRequestHandler<VerifyEmailOtpCommand, RequestResponse<VerifyEmailOtpResponse>>
{
    private readonly VerifyEmailOtpOrchestrator _orchestrator;

    public VerifyEmailOtpCommandHandler(VerifyEmailOtpOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public async Task<RequestResponse<VerifyEmailOtpResponse>> Handle(VerifyEmailOtpCommand request, CancellationToken cancellationToken)
    {
        return await _orchestrator.VerifyEmailOtpAsync(request, cancellationToken);
    }
}
