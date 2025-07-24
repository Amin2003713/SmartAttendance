using Shifty.Application.Dashboards.Requests.ProgressHistory;
using Shifty.Application.Dashboards.Responses.ProgressHistory;

namespace Shifty.Application.Dashboards.Queries.ProgressHistory;

public record ProjectProgressHistoryQuery : ProjectProgressHistoryRequest,
    IRequest<List<ProjectProgressHistoryResponse>>;