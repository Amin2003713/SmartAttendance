using Shifty.Application.Features.Dashboards.Requests.ProgressHistory;
using Shifty.Application.Features.Dashboards.Responses.ProgressHistory;

namespace Shifty.Application.Features.Dashboards.Queries.ProgressHistory;

public record ProjectProgressHistoryQuery : ProjectProgressHistoryRequest,
    IRequest<List<ProjectProgressHistoryResponse>>;