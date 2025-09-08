using Mapster;
using SmartAttendance.Application.Features.TaskTrack.Commands.DeleteTaskTrackReport;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.TaskTracks.Aggregate;
using SmartAttendance.Domain.TaskTracks.Events.TaskTrackReports;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.TaskTracks.Commands.DeleteTaskTrackReport;

public class DeleteTaskTrackReportCommandHandler(
    IEventReader<TaskTrack, Guid> eventReader,
    IdentityService identityService,
    ILogger<DeleteTaskTrackReportCommandHandler> logger,
    IEventWriter<TaskTrack, Guid> eventWriter,
    IStringLocalizer<DeleteTaskTrackReportCommandHandler> localizer
)
    : IRequestHandler<DeleteTaskTrackReportCommand>
{
    public async Task Handle(DeleteTaskTrackReportCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();

        logger.LogInformation("User {UserId} is attempting to delete report {ReportId} from Missions {AggregateId}",
            userId,
            request.ReportId,
            request.AggregateId);

        var taskTrack = await eventReader.GetSingleAsync(x => x.AggregateId == request.AggregateId,
            cancellationToken);

        if (taskTrack is null)
        {
            logger.LogWarning("Missions {AggregateId} not found", request.AggregateId);
            throw SmartAttendanceException.NotFound(localizer["No Missions reports found to delete."].Value);
        }

        var report = taskTrack.Reports.FirstOrDefault(r => r.ReportId == request.ReportId);

        if (report is null)
        {
            logger.LogWarning("Report {ReportId} not found in Missions {AggregateId}",
                request.ReportId,
                request.AggregateId);

            throw SmartAttendanceException.NotFound(localizer["Report not found for deletion."].Value);
        }

        var deleteEvent = report.Adapt<TaskTrackReportDeletedEvent>() with
        {
            AggregateId = request.AggregateId,
            Reported = DateTime.Now
        };

        taskTrack.DeleteReport(deleteEvent);

        await eventWriter.SaveAsync(taskTrack, cancellationToken);

        logger.LogInformation("User {UserId} successfully deleted report {ReportId} from Missions {AggregateId}",
            userId,
            request.ReportId,
            request.AggregateId);
    }
}