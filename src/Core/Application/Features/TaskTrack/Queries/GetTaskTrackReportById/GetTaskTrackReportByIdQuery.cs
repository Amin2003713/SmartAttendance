using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTrackReport;

namespace Shifty.Application.Features.TaskTrack.Queries.GetTaskTrackReportById;

public record GetTaskTrackReportByIdQuery(
    Guid AggregateId,
    Guid ReportId
) : IRequest<TaskTrackReportResponse>;