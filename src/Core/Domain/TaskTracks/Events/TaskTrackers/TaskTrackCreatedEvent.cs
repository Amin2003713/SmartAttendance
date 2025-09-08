using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Domain.TaskTracks.Events.TaskTrackers;

public record TaskTrackCreatedEvent(
    Guid AggregateId,
    string Title,
    string Description,
    Guid? WorkPackageId,
    PriorityType PriorityType,
    List<Guid> AssigneeId,
    Guid CreatedBy,
    DateTime StartDate,
    DateTime EndDate,
    TasksStatus Status
) : DomainEvent;