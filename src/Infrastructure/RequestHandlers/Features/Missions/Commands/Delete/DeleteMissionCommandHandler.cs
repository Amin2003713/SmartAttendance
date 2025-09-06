using Mapster;
using Shifty.Application.Features.Missions.Commands.Delete;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.Missions.Aggregate;
using Shifty.Domain.Missions.Events.TaskTrackers;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Missions.Commands.Delete;

public class DeleteMissionCommandHandler(
    IdentityService identityService,
    ILogger<DeleteMissionCommandHandler> logger,
    IStringLocalizer<DeleteMissionCommandHandler> localizer,
    IEventWriter<Mission, Guid> eventWriter,
    IEventReader<Mission, Guid> eventReader) : IRequestHandler<DeleteMissionCommand>
{
    public async Task Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();

        logger.LogInformation("Attempting to delete Missions with ID {AggregateId} by user {UserId}",
            request.AggregateId,
            userId);

        var mission = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.AggregateId && x.UserId == userId,
            cancellationToken: cancellationToken);

        if (mission is null)
        {
            logger.LogWarning("Missions {AggregateId} not found or access denied for user {UserId}",
                request.AggregateId,
                userId);

            throw ShiftyException.NotFound(localizer["No Missions reports found to delete."].Value);
        }

        var deleteEvent = request.Adapt<MissionDeletedEvent>() with
        {
            Reported = DateTime.Now
        };

        mission.Delete(deleteEvent);

        await eventWriter.SaveAsync(mission, cancellationToken);

        logger.LogInformation("Successfully deleted Missions with ID {AggregateId}", request.AggregateId);
    }
}