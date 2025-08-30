namespace TaskTracker.Domain.TaskTracks;

public class TaskTrackReport
{
    public Guid ReportId { get; set; }
    public string ReportDetail { get; set; }
    public Guid ReportCreatedBy { get; set; }
    public DateTime StartReportDate { get; set; }
    public DateTime EndReportDate { get; set; }
    public TimeSpan WorkTime { get; set; }
}