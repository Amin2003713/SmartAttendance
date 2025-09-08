using SmartAttendance.Application.Features.TaskTrack.Commands.CreateTaskTrack;
using SmartAttendance.Application.Features.TaskTrack.Commands.DeleteTaskTrack;
using SmartAttendance.Application.Features.TaskTrack.Commands.UpdateTaskTrack;
using SmartAttendance.Application.Features.TaskTrack.Queries.GetTackTracks;
using SmartAttendance.Application.Features.TaskTrack.Queries.GetTaskTrackById;
using SmartAttendance.Application.Features.TaskTrack.Requests.Commands.CreateTaskTrack;
using SmartAttendance.Application.Features.TaskTrack.Requests.Commands.UpdateTaskTrack;
using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackById;
using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTracks;
using SmartAttendance.Common.Utilities.PaginationHelpers;

namespace SmartAttendance.Api.Controllers.TaskTracks;

public class Task_TrackController : SmartAttendanceBaseController
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create TaskTracker")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Create([FromBody] CreateTaskTrackRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateTaskTrackCommand>(), cancellationToken);
    }


    [HttpPut]
    [SwaggerOperation(Summary = "Put TaskTracker")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Update([FromBody] UpdateTaskTrackRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateTaskTrackCommand>(), cancellationToken);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Delete TaskTracker")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task Delete(Guid aggregateId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTaskTrackCommand(aggregateId), cancellationToken);
    }

    [HttpGet("daily-report")]
    [SwaggerOperation("Get Task reporting for a project.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PagedResult<GetTaskTrackResponse>> GetDailyReporting(
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(new GetTaskTrackQuery(pageNumber, pageSize), cancellationToken);
    }


    [HttpGet("Get-Task-Track-By-Id")]
    [SwaggerOperation("Get task with its reports by ID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<GetTaskTrackByIdResponse> GetTaskWithReports(
        Guid aggregateId,
        CancellationToken cancellationToken = default)
    {
        return await Mediator.Send(new GetTaskTrackByIdQuery(aggregateId), cancellationToken);
    }
}