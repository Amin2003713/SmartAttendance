namespace Shifty.Domain.TaskTracks.Events.TaskTrackReports;

public record TaskTrackReportUpdatedEvent(
    Guid AggregateId,
    Guid ReportId,
    string ReportDetail,
    Guid ReportCreatedBy,
    DateTime StartReportDate,
    DateTime EndReportDate,
    TimeSpan WorkTime
) : DomainEvent;