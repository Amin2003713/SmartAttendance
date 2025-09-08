using SmartAttendance.Application.Features.TaskTrack.Commands.CreateTaskTrackReport;
using SmartAttendance.Application.Features.TaskTrack.Commands.DeleteTaskTrackReport;
using SmartAttendance.Application.Features.TaskTrack.Commands.UpdateTaskTrackReport;
using SmartAttendance.Application.Features.TaskTrack.Queries.GetTaskTrackReportById;
using SmartAttendance.Application.Features.TaskTrack.Requests.Commands.CreateTaskTrackReport;
using SmartAttendance.Application.Features.TaskTrack.Requests.Commands.UpdateTaskTrackReport;
using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;

namespace SmartAttendance.Api.Controllers.TaskTrackReports;

public class Task_Track_ReportController : SmartAttendanceBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create Task-Tracker-Report")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Create([FromBody] CreateTaskTrackReportRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateTaskTrackReportCommand>(), cancellationToken);
    }

    [HttpPut]
    [SwaggerOperation(Summary = "Put TaskTracker")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Update([FromBody] UpdateTaskTrackReportRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateTaskTrackReportCommand>(), cancellationToken);
    }


    [HttpDelete]
    [SwaggerOperation(Summary = "Delete TaskTracker")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Delete(Guid aggregateId, Guid reportId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTaskTrackReportCommand(aggregateId, reportId), cancellationToken);
    }

    [HttpGet("Get-Task-Track-Report-By-Id")]
    [SwaggerOperation("Get task with its reports by ID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<TaskTrackReportResponse> GetTaskWithReports(
        Guid aggregateId,
        Guid reportId,
        CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(new GetTaskTrackReportByIdQuery(aggregateId, reportId),
            cancellationToken);
    }
}