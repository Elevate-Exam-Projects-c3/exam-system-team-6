using exam_system.Features.Diplomas.GetStudentDashboard.Dtos;
using exam_system.Features.Diplomas.GetStudentDashboard.Queries;
using exam_system.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_system.Features.Diplomas.GetStudentDashboard.Controllers;

[ApiController]
[Route("api/students/me")]
[Authorize(Roles = "Student")]
public class StudentDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<EndpointResponse<StudentDashboardDto>>> GetDashboard(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetStudentDashboardQuery(),
            cancellationToken);

        var response = EndpointResponse<StudentDashboardDto>.FromResult(result);

        return StatusCode(response.StatusCode, response);
    }
}