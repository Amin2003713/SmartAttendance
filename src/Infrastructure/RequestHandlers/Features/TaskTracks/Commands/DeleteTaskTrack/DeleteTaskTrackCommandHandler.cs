using Mapster;
using SmartAttendance.Application.Features.TaskTrack.Commands.DeleteTaskTrack;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.TaskTracks.Aggregate;
using SmartAttendance.Domain.TaskTracks.Events.TaskTrackers;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.TaskTracks.Commands.DeleteTaskTrack;

public class DeleteTaskTrackCommandHandler(
    IdentityService identityService,
    IEventReader<TaskTrack, Guid> eventReader,
    IEventWriter<TaskTrack, Guid> eventWriter,
    ILogger<DeleteTaskTrackCommandHandler> logger,
    IStringLocalizer<DeleteTaskTrackCommandHandler> localizer
) : IRequestHandler<DeleteTaskTrackCommand>
{
    public async Task Handle(DeleteTaskTrackCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();

        logger.LogInformation("Attempting to delete Missions with ID {AggregateId} by user {UserId}",
            request.AggregateId,
            userId);

        var taskTrack = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.AggregateId && x.UserId == userId,
            cancellationToken);

        if (taskTrack is null)
        {
            logger.LogWarning("Missions {AggregateId} not found or access denied for user {UserId}",
                request.AggregateId,
                userId);

            throw SmartAttendanceException.NotFound(localizer["No Missions reports found to delete."].Value);
        }

        var deleteEvent = request.Adapt<TaskTrackDeletedEvent>() with
        {
            Reported = DateTime.Now
        };

        taskTrack.Delete(deleteEvent);

        await eventWriter.SaveAsync(taskTrack, cancellationToken);

        logger.LogInformation("Successfully deleted Missions with ID {AggregateId}", request.AggregateId);
    }
}