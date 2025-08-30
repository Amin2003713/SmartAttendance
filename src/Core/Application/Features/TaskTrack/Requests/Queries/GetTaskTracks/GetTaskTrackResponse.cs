using Shifty.Common.General.Enums;

namespace TaskTracker.Application.TaskTrack.Requests.Queries.GetTaskTracks;

public class GetTaskTrackResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PriorityType PriorityType { get; set; }
    public List<Guid> AssigneeId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TasksStatus Status { get; set; }
}