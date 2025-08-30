using Shifty.Common.General.Enums;

namespace Shifty.Application.Features.TaskTrack.Requests.Commands.UpdateTaskTrack;

public class UpdateTaskTrackRequest
{
    public Guid AggregateId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PriorityType PriorityType { get; set; }
    public List<Guid> AssigneeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TasksStatus Status { get; set; }
}