using Mapster;
using SmartAttendance.Application.Features.Missions.Commands.Create;
using SmartAttendance.Application.Interfaces.Base.EventInterface;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Missions.Aggregate;
using SmartAttendance.Domain.Missions.Events.TaskTrackers;

namespace SmartAttendance.RequestHandlers.Features.Missions.Commands.Create;

public class CreateMissionCommandHandler(
    ILogger<CreateMissionCommandHandler> logger,
    IStringLocalizer<CreateMissionCommandHandler> localizer,
    IEventWriter<Mission, Guid> eventWriter
) : IRequestHandler<CreateMissionCommand>
{
    public async Task Handle(CreateMissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var mission = Mission.New(
                request.Adapt<MissionCreatedEvent>() with
                {
                    AggregateId = Guid.CreateVersion7(DateTimeOffset.Now)
                }
            );


            await eventWriter.SaveAsync(mission, cancellationToken);

            logger.LogInformation("MissionES {MissionId} created via event sourcing.", mission.AggregateId);
        }
        catch (SmartAttendanceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating mission via ES ");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while creating the mission."].Value);
        }
    }
}