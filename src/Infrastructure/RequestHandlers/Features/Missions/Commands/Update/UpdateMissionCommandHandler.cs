using Mapster;
using SmartAttendance.Application.Features.Missions.Commands.Update;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Missions.Aggregate;
using SmartAttendance.Domain.Missions.Events.TaskTrackers;

namespace SmartAttendance.RequestHandlers.Features.Missions.Commands.Update;

public class UpdateMissionCommandHandler(
    IEventReader<Mission, Guid> eventReader,
    IEventWriter<Mission, Guid> eventWriter,
    ILogger<UpdateMissionCommandHandler> logger,
    IStringLocalizer<UpdateMissionCommandHandler> localizer
) : IRequestHandler<UpdateMissionCommand>
{
    public async Task Handle(UpdateMissionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating mission {MissionId} .",
            request.AggregateId);

        if (!await eventReader.ExistsAsync(request.AggregateId, cancellationToken))
            throw SmartAttendanceException.NotFound(localizer["Mission not found."].Value);

        var mission = await eventReader.GetSingleAsync(aq => aq.AggregateId == request.AggregateId,
            cancellationToken);

        if (mission == null)
            throw SmartAttendanceException.NotFound();

        mission.Update(request.Adapt<MissionUpdatedEvent>());

        try
        {
            await eventWriter.SaveAsync(mission, cancellationToken);

            logger.LogInformation("Mission {MissionId} updated successfully.", request.AggregateId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating mission {MissionId}.", request.AggregateId);
            throw SmartAttendanceException.InternalServerError(localizer["An error occurred while updating the mission."].Value);
        }
    }
}