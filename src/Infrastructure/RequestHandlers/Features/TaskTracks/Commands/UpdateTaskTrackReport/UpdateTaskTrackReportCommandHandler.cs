using Mapster;
using Shifty.Application.Features.TaskTrack.Commands.UpdateTaskTrackReport;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Domain.TaskTracks.Events.TaskTrackReports;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Commands.UpdateTaskTrackReport;

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
            logger.LogWarning("TaskTrack {TackTrackId} not found.", request.TaskTrackId);
            throw ShiftyException.NotFound(localizer["TaskTrack not found."].Value);
        }

        var userId = identityService.GetUserId<Guid>();

        var taskTrack = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.TaskTrackId && x.UserId == identityService.GetUserId<Guid>(),
            cancellationToken: cancellationToken);

        if (taskTrack is null)
            throw ShiftyException.NotFound(localizer["TaskTrack not found."].Value);

        var report = taskTrack.Reports.FirstOrDefault(r => r.ReportId == request.ReportId);

        if (report == null)
        {
            logger.LogWarning("Report {ReportId} not found in TaskTrack {TaskTrackId}.",
                request.ReportId,
                request.TaskTrackId);

            throw ShiftyException.NotFound(localizer["Report not found."].Value);
        }

        if (report.ReportCreatedBy != userId)
        {
            logger.LogWarning("User {UserId} tried to update a report not owned by them.", userId);
            throw ShiftyException.Forbidden(localizer["You can only update your own report."].Value);
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