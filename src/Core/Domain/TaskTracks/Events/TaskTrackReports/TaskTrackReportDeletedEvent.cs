namespace SmartAttendance.Domain.TaskTracks.Events.TaskTrackReports;

public record TaskTrackReportDeletedEvent(
    Guid AggregateId,
    Guid ReportId
) : DomainEvent;