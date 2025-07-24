using Shifty.Application.Dashboards.Responses.ResourceChartDat;

namespace Shifty.Application.Dashboards.Queries.BudgetDistributionCharts;

public record BudgetDistributionChartQuery : IRequest<List<BudgetDistributionChartResponse>>;