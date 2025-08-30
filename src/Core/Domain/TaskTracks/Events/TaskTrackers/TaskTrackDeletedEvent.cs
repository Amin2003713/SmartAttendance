namespace Shifty.Domain.TaskTracks.Events.TaskTrackers;

public record TaskTrackDeletedEvent(
    Guid AggregateId
) : DomainEvent;