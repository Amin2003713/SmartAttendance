using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;
using Shifty.Common.General.Enums;

namespace Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackById;

public class GetTaskTrackByIdResponse
{
    public Guid AggregateId { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PriorityType PriorityType { get; set; }
    public List<Guid> AssigneeId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TasksStatus Status { get; set; }

    public List<TaskTrackReportResponse> Reports { get; set; }
}