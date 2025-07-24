using Shifty.Application.Dashboards.Queries.ProgressHistory;
using Shifty.Application.Dashboards.Responses.ProgressHistory;

namespace Shifty.RequestHandlers.Dashboards.Queries.ProjectProgressHistory;

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