namespace SmartAttendance.Domain.Missions.Events.TaskTrackers;

public record MissionDeletedEvent(
    Guid AggregateId
) : DomainEvent;