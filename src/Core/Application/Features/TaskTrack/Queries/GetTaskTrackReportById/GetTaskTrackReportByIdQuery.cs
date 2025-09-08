using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;

namespace SmartAttendance.Application.Features.TaskTrack.Queries.GetTaskTrackReportById;

public record GetTaskTrackReportByIdQuery(
    Guid AggregateId,
    Guid ReportId
) : IRequest<TaskTrackReportResponse>;