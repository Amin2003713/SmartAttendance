using Mapster;
using SmartAttendance.Application.Features.TaskTrack.Commands.UpdateTaskTrackReport;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.TaskTracks.Aggregate;
using SmartAttendance.Domain.TaskTracks.Events.TaskTrackReports;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.TaskTracks.Commands.UpdateTaskTrackReport;

public class UpdateTaskTrackReportCommandHandler(
    IEventReader<TaskTrack, Guid> eventReader,
    IEventWriter<TaskTrack, Guid> eventWriter,
    ILogger<UpdateTaskTrackReportCommandHandler> logger,
    IdentityService identityService,
    IStringLocalizer<UpdateTaskTrackReportCommandHandler> localizer
) : IRequestHandler<UpdateTaskTrackReportCommand>
{
    public async Task Handle(UpdateTaskTrackReportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating taskTrackReport {ReportId}.",
            request.ReportId);

        if (!await eventReader.ExistsAsync(request.TaskTrackId, cancellationToken))
        {
            logger.LogWarning("Missions {TackTrackId} not found.", request.TaskTrackId);
            throw SmartAttendanceException.NotFound(localizer["Missions not found."].Value);
        }

        var userId = identityService.GetUserId<Guid>();

        var taskTrack = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.TaskTrackId && x.UserId == identityService.GetUserId<Guid>(),
            cancellationToken);

        if (taskTrack is null)
            throw SmartAttendanceException.NotFound(localizer["Missions not found."].Value);

        var report = taskTrack.Reports.FirstOrDefault(r => r.ReportId == request.ReportId);

        if (report == null)
        {
            logger.LogWarning("Report {ReportId} not found in Missions {TaskTrackId}.",
                request.ReportId,
                request.TaskTrackId);

            throw SmartAttendanceException.NotFound(localizer["Report not found."].Value);
        }

        if (report.ReportCreatedBy != userId)
        {
            logger.LogWarning("User {UserId} tried to update a report not owned by them.", userId);
            throw SmartAttendanceException.Forbidden(localizer["You can only update your own report."].Value);
        }

        var updateEvent = request.Adapt<TaskTrackReportUpdatedEvent>() with
        {
            AggregateId = taskTrack.AggregateId,
            Reported = DateTime.UtcNow,
            WorkTime = request.WorkTime,
            ReportCreatedBy = userId
        };

        taskTrack.UpdateReport(updateEvent);

        try
        {
            await eventWriter.SaveAsync(taskTrack, cancellationToken);
            logger.LogInformation("TaskTrackReport {ReportId} updated successfully.", request.ReportId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating taskTrackReport {ReportId}.", request.ReportId);
            throw;
        }
    }
}