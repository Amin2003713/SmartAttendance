using Mapster;
using SmartAttendance.Application.Features.TaskTrack.Commands.CreateTaskTrackReport;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.TaskTracks.Aggregate;
using SmartAttendance.Domain.TaskTracks.Events.TaskTrackReports;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.TaskTracks.Commands.CreateTaskTrackReport;

public class CreateTaskTrackReportCommandHandler(
    ILogger<CreateTaskTrackReportCommandHandler> logger,
    IEventReader<TaskTrack, Guid> eventReader,
    IEventWriter<TaskTrack, Guid> eventWriter,
    IStringLocalizer<CreateTaskTrackReportCommandHandler> localizer,
    IdentityService identityService
) : IRequestHandler<CreateTaskTrackReportCommand>
{
    public async Task Handle(CreateTaskTrackReportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Adding TaskTrackReport , TaskTrackId {TaskTrackId}.",
            request.TaskTrackId);

        try
        {
            var userId = identityService.GetUserId<Guid>();


            var taskTrack = await eventReader.LoadAsync(request.TaskTrackId, cancellationToken);


            if (taskTrack!.CreatedBy != userId && !taskTrack.AssigneeId.Contains(userId))
            {
                throw SmartAttendanceException.BadRequest(localizer["You cant not set your work log."].Value);
            }

            var reportEvent = request.Adapt<TaskTrackReportCreatedEvent>() with
            {
                ReportId = Guid.CreateVersion7(DateTime.UtcNow),
                AggregateId = taskTrack.AggregateId,
                ReportCreatedBy = userId,
                Reported = DateTime.Now
            };

            taskTrack.AddReport(reportEvent);

            await eventWriter.SaveAsync(taskTrack, cancellationToken);

            logger.LogInformation("TaskTrackReport added to Missions {TaskTrackId} via event sourcing.",
                taskTrack.AggregateId);
        }
        catch (SmartAttendanceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding TaskTrackReport ");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while adding the task report."].Value);
        }
    }
}