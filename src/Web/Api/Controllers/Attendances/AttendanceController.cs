using Microsoft.AspNetCore.Mvc;
using SmartAttendance.Application.Features.Attendances.Commands;
using SmartAttendance.Application.Features.Attendances.Queries;
using SmartAttendance.Application.Features.Attendances.Queries.GetByPlan;
using SmartAttendance.Application.Features.Attendances.Responses;

namespace SmartAttendance.Api.Controllers.Plans;

[ApiController]
public class AttendanceController : SmartAttendanceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 200)]
    public async Task<Guid> Create([FromBody] CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{attendanceId:guid}/status")]
    [ProducesResponseType(200)]
    public async Task UpdateStatus(Guid attendanceId, [FromBody] UpdateAttendanceStatusCommand request, CancellationToken cancellationToken)
    {
        request.AttendanceId = attendanceId;
        await Mediator.Send(request, cancellationToken);
    }

    [HttpGet("by-plan/{planId:guid}")]
    [ProducesResponseType(typeof(List<GetAttendanceInfoResponse>), 200)]
    public async Task<List<GetAttendanceInfoResponse>> GetByPlan(Guid planId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAttendancesByPlanQuery
            {
                PlanId = planId
            },
            cancellationToken);
    }

    [HttpGet("by-student/{studentId:guid}")]
    [ProducesResponseType(typeof(List<GetAttendanceInfoResponse>), 200)]
    public async Task<List<GetAttendanceInfoResponse>> GetByStudent(Guid studentId, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetAttendancesByStudentQuery
            {
                StudentId = studentId
            },
            cancellationToken);
    }
}