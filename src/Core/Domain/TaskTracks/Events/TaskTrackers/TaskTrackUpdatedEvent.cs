using System.Threading.Tasks;
using Shifty.Common.General.Enums;

namespace Shifty.Domain.TaskTracks.Events.TaskTrackers;

public record TaskTrackUpdatedEvent(
    Guid AggregateId,
    string Title,
    string Description,
    PriorityType PriorityType,
    List<Guid> AssigneeId,
    Guid CreatedBy,
    DateTime StartDate,
    DateTime EndDate,
    TasksStatus Status
) : DomainEvent;