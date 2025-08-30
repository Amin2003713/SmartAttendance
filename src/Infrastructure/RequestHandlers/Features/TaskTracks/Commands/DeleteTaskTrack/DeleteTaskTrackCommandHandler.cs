using Mapster;
using Shifty.Application.Features.TaskTrack.Commands.DeleteTaskTrack;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Domain.TaskTracks.Events.TaskTrackers;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Commands.DeleteTaskTrack;

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

        logger.LogInformation("Attempting to delete TaskTrack with ID {AggregateId} by user {UserId}",
            request.AggregateId,
            userId);

        var taskTrack = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.AggregateId && x.UserId == userId,
            cancellationToken: cancellationToken);

        if (taskTrack is null)
        {
            logger.LogWarning("TaskTrack {AggregateId} not found or access denied for user {UserId}",
                request.AggregateId,
                userId);

            throw ShiftyException.NotFound(localizer["No TaskTrack reports found to delete."].Value);
        }

        var deleteEvent = request.Adapt<TaskTrackDeletedEvent>() with
        {
            Reported = DateTime.Now
        };

        taskTrack.Delete(deleteEvent);

        await eventWriter.SaveAsync(taskTrack, cancellationToken);

        logger.LogInformation("Successfully deleted TaskTrack with ID {AggregateId}", request.AggregateId);
    }
}