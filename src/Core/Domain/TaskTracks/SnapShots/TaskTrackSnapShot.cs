using Shifty.Common.General.Enums;

namespace Shifty.Domain.TaskTracks.SnapShots;

public class TaskTrackSnapShot : ISnapshot<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public PriorityType PriorityType { get; set; }
    public List<Guid> AssigneeId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TasksStatus Status { get; set; }

    public List<TaskTrackReport> Reports { get; set; }
    public DateTime Reported { get; set; }

    public Guid AggregateId { get; init; }
    public int Version { get; init; }
    public DateTime LastAction { get; init; }
    public bool Deleted { get; init; }
}