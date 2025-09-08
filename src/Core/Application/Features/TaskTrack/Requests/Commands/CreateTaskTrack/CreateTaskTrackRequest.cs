using SmartAttendance.Common.General.Enums;

namespace SmartAttendance.Application.Features.TaskTrack.Requests.Commands.CreateTaskTrack;

public class CreateTaskTrackRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public PriorityType PriorityType { get; set; }
    public List<Guid> AssigneeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TasksStatus Status { get; set; }
}