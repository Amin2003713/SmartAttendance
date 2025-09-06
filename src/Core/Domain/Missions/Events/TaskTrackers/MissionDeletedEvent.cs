namespace Shifty.Domain.Missions.Events.TaskTrackers;

public record MissionDeletedEvent(
    Guid AggregateId
) : DomainEvent;