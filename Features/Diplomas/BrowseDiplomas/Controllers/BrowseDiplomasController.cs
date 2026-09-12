using exam_system.Features.Diplomas.BrowseDiplomas.Dtos;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Controllers;


[ApiController]
[Route("api/diplomas")]
//[Authorize(Roles = "Student")]
public class BrowseDiplomasController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrowseDiplomasController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<ActionResult<
        EndpointResponse<PaginatedResult<BrowseDiplomaDto>>>> Browse(
        [FromQuery] BrowseDiplomasQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        var response =
            EndpointResponse<PaginatedResult<BrowseDiplomaDto>>
                .FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
    
}