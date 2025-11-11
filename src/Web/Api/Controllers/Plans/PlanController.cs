using Microsoft.AspNetCore.Mvc;
using SmartAttendance.Application.Features.Plans.Commands.Create;
using SmartAttendance.Application.Features.Plans.Commands.Update;
using SmartAttendance.Application.Features.Plans.Commands.Delete;
using SmartAttendance.Application.Features.Plans.Queries.GetAll;
using SmartAttendance.Application.Features.Plans.Queries.GetById;
using SmartAttendance.Application.Features.Plans.Queries.GetByDate;
using SmartAttendance.Application.Features.Plans.Responses;
using Mapster;
using SmartAttendance.Application.Features.Plans.Commands.ReschedulePlan;
using SmartAttendance.Application.Features.Plans.Queries.ListAllByStudentMajor;
using SmartAttendance.Application.Features.Plans.Request.Commands.ReschedulePlan;
using SmartAttendance.Application.Features.Plans.Request.Commands.Update;

namespace SmartAttendance.Api.Controllers.Plans;

[ApiController]
public class PlanController : SmartAttendanceBaseController
{
    /// <summary>
    /// Creates a new plan with subjects and teachers.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task CreatePlan([FromBody] CreatePlanCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
    }

    /// <summary>
    /// Updates an existing plan and returns its updated info.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(GetPlanInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task UpdatePlan([FromBody] UpdatePlanRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdatePlanCommand>(), cancellationToken);
    }

    /// <summary>
    /// Deletes a plan by its ID and returns the deleted plan ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task DeletePlan(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePlanCommand(id), cancellationToken);
    }

    /// <summary>
    /// Reschedules an existing plan and returns the updated plan details.
    /// </summary>
    [HttpPut("reschedule")]
    [ProducesResponseType(typeof(GetPlanInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task ReschedulePlan([FromBody] ReschedulePlanRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<ReschedulePlanCommand>(), cancellationToken);
    }

    /// <summary>
    /// Gets all available plans.
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<GetPlanInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetPlanInfoResponse>> GetAllPlans(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAllPlanQuery(), cancellationToken);
    }

    /// <summary>
    /// Gets a plan by its unique ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetPlanInfoResponse), StatusCodes.Status200OK)]
    public async Task<GetPlanInfoResponse> GetPlanById(Guid id, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetByIdQuery
            {
                Id = id
            },
            cancellationToken);
    }

    /// <summary>
    /// Lists all plans for a specific student.
    /// </summary>
    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(typeof(List<GetPlanInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetPlanInfoResponse>> ListByStudent(Guid studentId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new ListAllByStudentQuery
            {
                StudentId = studentId
            },
            cancellationToken);
    }

    /// <summary>
    /// Gets plans in a specific date range (filtered by roles internally).
    /// </summary>
    [HttpGet("by-date-range")]
    [ProducesResponseType(typeof(List<GetPlanInfoResponse>), StatusCodes.Status200OK)]
    public async Task<List<GetPlanInfoResponse>> GetPlansByDateRange(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetPlanByDateRangeQuery
            {
                From = from,
                To = to
            },
            cancellationToken);
    }
}