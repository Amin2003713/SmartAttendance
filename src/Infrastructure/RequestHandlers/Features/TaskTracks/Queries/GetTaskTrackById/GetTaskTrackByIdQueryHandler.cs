using Mapster;
using Shifty.Application.Features.TaskTrack.Queries.GetTaskTrackById;
using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackById;
using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Queries.GetTaskTrackById;

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
            throw ShiftyException.NotFound("Task not found");
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