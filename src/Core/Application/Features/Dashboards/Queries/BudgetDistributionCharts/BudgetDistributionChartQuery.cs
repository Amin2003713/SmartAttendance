using SmartAttendance.Application.Features.Dashboards.Responses.ResourceChartDat;

namespace SmartAttendance.Application.Features.Dashboards.Queries.BudgetDistributionCharts;

public record BudgetDistributionChartQuery : IRequest<List<BudgetDistributionChartResponse>>;