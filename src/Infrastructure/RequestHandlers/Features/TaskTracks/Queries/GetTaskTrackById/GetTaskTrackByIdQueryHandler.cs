using Mapster;
using SmartAttendance.Application.Features.TaskTrack.Queries.GetTaskTrackById;
using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackById;
using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.TaskTracks.Aggregate;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.TaskTracks.Queries.GetTaskTrackById;

public class GetTaskTrackByIdQueryHandler(
    IdentityService identityService,
    IEventReader<TaskTrack, Guid> eventReader,
    ILogger<GetTaskTrackByIdQueryHandler> logger
)
    : IRequestHandler<GetTaskTrackByIdQuery, GetTaskTrackByIdResponse>
{
    public async Task<GetTaskTrackByIdResponse> Handle(
        GetTaskTrackByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();
        logger.LogInformation("User {UserId} requested Missions with ID {AggregateId} ",
            userId,
            request.AggregateId);


        var events = await eventReader.LoadEventsAsync(request.AggregateId, cancellationToken);

        if (events is null || events.Count == 0)
        {
            logger.LogWarning("No events found for Missions {AggregateId}", request.AggregateId);
            throw SmartAttendanceException.NotFound("Task not found");
        }

        var taskTrack = new TaskTrack();
        taskTrack.LoadFromHistory(events);


        var response = taskTrack.Adapt<GetTaskTrackByIdResponse>();
        response.Reports = taskTrack.Reports.Adapt<List<TaskTrackReportResponse>>();
        response.AggregateId = taskTrack.AggregateId;

        logger.LogInformation(
            "Successfully retrieved Missions {AggregateId} with {ReportCount} reports for user {UserId}",
            request.AggregateId,
            response.Reports.Count,
            userId);

        return response;
    }
}