using Mapster;
using Shifty.Application.Features.Missions.Commands.Create;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Domain.Missions.Aggregate;
using Shifty.Domain.Missions.Events.TaskTrackers;

namespace Shifty.RequestHandlers.Features.Missions.Commands.Create;

public class CreateMissionCommandHandler(
    ILogger<CreateMissionCommandHandler> logger,
    IStringLocalizer<CreateMissionCommandHandler> localizer,
    IEventWriter<Mission, Guid> eventWriter) : IRequestHandler<CreateMissionCommand>
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
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating mission via ES ");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while creating the mission."].Value);
        }
    }
}