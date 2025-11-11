using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace SmartAttendance.Api.Controllers.Plans;

[Route("api/[controller]")]
[ApiController]
public class PlanEnrollmentController : SmartAttendanceBaseController
{
    /// <summary>
    /// Enrolls a student into a plan. If capacity is full, enrollment is waitlisted.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Guid> CreateEnrollment([FromBody] CreatePlanEnrollmentCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    /// <summary>
    /// Deletes an enrollment. Automatically promotes oldest waitlisted enrollment if capacity allows.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeleteEnrollment([FromBody] DeletePlanEnrollmentCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
    }

    /// <summary>
    /// Lists all enrollments for a specific plan.
    /// </summary>
    [HttpGet("by-plan/{planId:guid}")]
    [ProducesResponseType(typeof(List<GetEnrollmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<GetEnrollmentResponse>> GetEnrollmentsByPlan(Guid planId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetPlanEnrollmentsQuery
            {
                PlanId = planId
            },
            cancellationToken);
    }

    /// <summary>
    /// Lists all enrollments for a specific student.
    /// </summary>
    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(typeof(List<GetEnrollmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<GetEnrollmentResponse>> GetEnrollmentsByStudent(Guid studentId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetStudentEnrollmentsQuery
            {
                StudentId = studentId
            },
            cancellationToken);
    }
}