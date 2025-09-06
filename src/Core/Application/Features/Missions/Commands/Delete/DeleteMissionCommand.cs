namespace Shifty.Application.Features.Missions.Commands.Delete;

public record DeleteMissionCommand(Guid AggregateId) : IRequest;