using exam_system.Features.Diplomas.GetDiplomaDetail.Dtos;
using exam_system.Features.Diplomas.GetDiplomaDetail.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.GetDiplomaDetail.Controllers;

[ApiController] 
[Route("api/diplomas")]
[Authorize(Roles = "Student")]
public class ViewDiplomaDetailController : ControllerBase
{
    private readonly IMediator _mediator;

    public ViewDiplomaDetailController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<
        EndpointResponse<ViewDiplomaDetailDto>>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new ViewDiplomaDetailQuery(id);

        var result = await _mediator.Send(
            query,
            cancellationToken);

        var response =
            EndpointResponse<ViewDiplomaDetailDto>
                .FromResult(result);

        return StatusCode(
            response.StatusCode,
            response);
    }
}
