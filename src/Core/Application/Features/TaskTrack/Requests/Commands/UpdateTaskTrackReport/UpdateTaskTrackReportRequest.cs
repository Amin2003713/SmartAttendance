namespace Shifty.Application.Features.TaskTrack.Requests.Commands.UpdateTaskTrackReport;

public class UpdateTaskTrackReportRequest
{
    public Guid ReportId { get; set; }

    public Guid TaskTrackId { get; set; }

    public string ReportDetail { get; set; }

    public DateTime StartReportDate { get; set; }

    public DateTime EndReportDate { get; set; }

    public TimeSpan WorkTime { get; set; }
}