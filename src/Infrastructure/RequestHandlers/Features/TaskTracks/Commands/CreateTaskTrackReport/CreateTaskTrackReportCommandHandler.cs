using Mapster;
using Shifty.Application.Features.TaskTrack.Commands.CreateTaskTrackReport;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Domain.TaskTracks.Events.TaskTrackReports;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Commands.CreateTaskTrackReport;

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
                throw ShiftyException.BadRequest(localizer["You cant not set your work log."].Value);
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
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding TaskTrackReport ");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while adding the task report."].Value);
        }
    }
}