using SmartAttendance.Application.Features.Dashboards.Requests.ProgressHistory;
using SmartAttendance.Application.Features.Dashboards.Responses.ProgressHistory;

namespace SmartAttendance.Application.Features.Dashboards.Queries.ProgressHistory;

public record ProjectProgressHistoryQuery : ProjectProgressHistoryRequest,
    IRequest<List<ProjectProgressHistoryResponse>>;