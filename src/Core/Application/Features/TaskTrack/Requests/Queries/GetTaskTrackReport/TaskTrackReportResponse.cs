namespace SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;

public class TaskTrackReportResponse
{
    public Guid AggregateId { get; set; }
    public Guid ReportId { get; set; }
    public string ReportDetail { get; set; }
    public Guid ReportCreatedBy { get; set; }
    public DateTime StartReportDate { get; set; }
    public DateTime EndReportDate { get; set; }
    public TimeSpan WorkTime { get; set; }
}