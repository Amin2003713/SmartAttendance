using Shifty.Application.Features.Dashboards.Responses.ResourceChartDat;

namespace Shifty.Application.Features.Dashboards.Queries.BudgetDistributionCharts;

public record BudgetDistributionChartQuery : IRequest<List<BudgetDistributionChartResponse>>;