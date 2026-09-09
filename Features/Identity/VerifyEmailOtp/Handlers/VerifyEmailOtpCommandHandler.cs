using MediatR;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class VerifyEmailOtpCommandHandler : IRequestHandler<VerifyEmailOtpCommand, RequestResponse<VerifyEmailOtpResponse>>
{
    private readonly IVerifyEmailOtpOrchestrator _orchestrator;

    public VerifyEmailOtpCommandHandler(IVerifyEmailOtpOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    public Task<RequestResponse<VerifyEmailOtpResponse>> Handle(VerifyEmailOtpCommand request, CancellationToken cancellationToken)
        => _orchestrator.VerifyAsync(request, cancellationToken);
}
