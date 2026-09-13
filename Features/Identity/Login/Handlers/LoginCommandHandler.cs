using MediatR;
using exam_system.Features.Identity.Login.Commands;
using exam_system.Features.Identity.Login.Orchestrators;
using exam_system.Features.Shared;

namespace exam_system.Features.Identity.Login.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, RequestResponse<LoginResponse>>
{
    private readonly IMediator _mediator;

    public LoginCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<RequestResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _mediator.Send(new LoginOrchestrator
        {
            Email = request.Email,
            Password = request.Password
        }, cancellationToken);
    }
}
