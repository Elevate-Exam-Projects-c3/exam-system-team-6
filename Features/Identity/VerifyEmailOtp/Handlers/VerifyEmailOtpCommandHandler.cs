using MediatR;
using exam_system.Features.Identity.VerifyEmailOtp;
using exam_system.Features.Identity.VerifyEmailOtp.Commands;
using exam_system.Features.Identity.VerifyEmailOtp.Orchestrators;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.VerifyEmailOtp.Handlers;

public class VerifyEmailOtpCommandHandler : IRequestHandler<VerifyEmailOtpCommand, RequestResponse<VerifyEmailOtpResponse>>
{
    private readonly IMediator _mediator;

    public VerifyEmailOtpCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<RequestResponse<VerifyEmailOtpResponse>> Handle(
        VerifyEmailOtpCommand request, CancellationToken cancellationToken)
        => _mediator.Send(new VerifyEmailOtpOrchestrator
        {
            Email = request.Email,
            Otp = request.Otp
        }, cancellationToken);
}
