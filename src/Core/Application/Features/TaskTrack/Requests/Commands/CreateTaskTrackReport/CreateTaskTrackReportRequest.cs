namespace Shifty.Application.Features.TaskTrack.Requests.Commands.CreateTaskTrackReport;

public class CreateTaskTrackReportRequest
{
    public Guid TaskTrackId { get; set; }

    public string ReportDetail { get; set; }

    public DateTime StartReportDate { get; set; }

    public DateTime EndReportDate { get; set; }

    public TimeSpan WorkTime { get; set; }
}