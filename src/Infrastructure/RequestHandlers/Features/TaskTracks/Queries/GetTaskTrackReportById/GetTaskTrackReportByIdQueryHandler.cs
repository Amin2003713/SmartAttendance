using Mapster;
using Shifty.Application.Features.TaskTrack.Queries.GetTaskTrackReportById;
using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Queries.GetTaskTrackReportById;

public class GetTaskTrackReportByIdQueryHandler(
    IdentityService identityService,
    IStringLocalizer<GetTaskTrackReportByIdQueryHandler> localizer,
    IEventReader<TaskTrack, Guid> eventReader,
    ILogger<GetTaskTrackReportByIdQueryHandler> logger
)
    : IRequestHandler<GetTaskTrackReportByIdQuery, TaskTrackReportResponse>
{
    public async Task<TaskTrackReportResponse> Handle(
        GetTaskTrackReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();
        logger.LogInformation("Fetching report {ReportId} from Missions {AggregateId} by user {UserId}",
            request.ReportId,
            request.AggregateId,
            userId);

        var taskTrack = await eventReader.GetSingleAsync(x => x.AggregateId == request.AggregateId,
            cancellationToken: cancellationToken);

        if (taskTrack is null)
        {
            logger.LogWarning("Missions {AggregateId} not found.", request.AggregateId);
            throw ShiftyException.NotFound(localizer["No Missions found."].Value);
        }

        if (taskTrack.CreatedBy != userId && !taskTrack.AssigneeId.Contains(userId))
        {
            logger.LogWarning("User {UserId} attempted to access Missions {AggregateId} without permission.",
                userId,
                request.AggregateId);

            throw ShiftyException.BadRequest(localizer["You can't see logs of this task."].Value);
        }

        var report = taskTrack.Reports.FirstOrDefault(r => r.ReportId == request.ReportId);

        if (report is null)
        {
            logger.LogWarning("Report {ReportId} not found in Missions {AggregateId}.",
                request.ReportId,
                request.AggregateId);

            throw ShiftyException.NotFound(localizer["Report not found."].Value);
        }

        var response = report.Adapt<TaskTrackReportResponse>();
        response.AggregateId = request.AggregateId;

        logger.LogInformation("Successfully retrieved report {ReportId} for Missions {AggregateId}.",
            request.ReportId,
            request.AggregateId);

        return response;
    }
}