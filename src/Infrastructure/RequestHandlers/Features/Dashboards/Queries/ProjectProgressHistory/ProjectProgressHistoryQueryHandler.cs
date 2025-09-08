using SmartAttendance.Application.Features.Dashboards.Queries.ProgressHistory;
using SmartAttendance.Application.Features.Dashboards.Responses.ProgressHistory;

namespace SmartAttendance.RequestHandlers.Features.Dashboards.Queries.ProjectProgressHistory;

public class
    ProjectProgressHistoryQueryHandler : IRequestHandler<ProjectProgressHistoryQuery,
    List<ProjectProgressHistoryResponse>>
{
    public Task<List<ProjectProgressHistoryResponse>> Handle(
        ProjectProgressHistoryQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}