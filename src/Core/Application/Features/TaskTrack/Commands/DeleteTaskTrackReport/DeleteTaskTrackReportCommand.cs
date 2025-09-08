namespace SmartAttendance.Application.Features.TaskTrack.Commands.DeleteTaskTrackReport;

public record DeleteTaskTrackReportCommand(
    Guid AggregateId,
    Guid ReportId
) : IRequest;