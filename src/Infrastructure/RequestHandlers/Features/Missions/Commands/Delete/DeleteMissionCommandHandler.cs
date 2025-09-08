using Mapster;
using SmartAttendance.Application.Features.Missions.Commands.Delete;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Missions.Aggregate;
using SmartAttendance.Domain.Missions.Events.TaskTrackers;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Missions.Commands.Delete;

public class DeleteMissionCommandHandler(
    IdentityService identityService,
    ILogger<DeleteMissionCommandHandler> logger,
    IStringLocalizer<DeleteMissionCommandHandler> localizer,
    IEventWriter<Mission, Guid> eventWriter,
    IEventReader<Mission, Guid> eventReader
) : IRequestHandler<DeleteMissionCommand>
{
    public async Task Handle(DeleteMissionCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();

        logger.LogInformation("Attempting to delete Missions with ID {AggregateId} by user {UserId}",
            request.AggregateId,
            userId);

        var mission = await eventReader.GetSingleAsync(
            x => x.AggregateId == request.AggregateId && x.UserId == userId,
            cancellationToken);

        if (mission is null)
        {
            logger.LogWarning("Missions {AggregateId} not found or access denied for user {UserId}",
                request.AggregateId,
                userId);

            throw SmartAttendanceException.NotFound(localizer["No Missions reports found to delete."].Value);
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