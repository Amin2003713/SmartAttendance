using SmartAttendance.Application.Features.Dashboards.Queries.BudgetDistributionCharts;
using SmartAttendance.Application.Features.Dashboards.Responses.ResourceChartDat;

namespace SmartAttendance.RequestHandlers.Features.Dashboards.Queries.BudgetDistributionCharts;

public class
    GetBudgetDistributionChartQueryHandler : IRequestHandler<BudgetDistributionChartQuery,
    List<BudgetDistributionChartResponse>>
{
    public Task<List<BudgetDistributionChartResponse>> Handle(
        BudgetDistributionChartQuery request,
        CancellationToken cancellationToken)
    {
        var dummyData = new List<BudgetDistributionChartResponse>
        {
            new()
            {
                Key = "humanResource",
                Budget = 275
            },
            new()
            {
                Key = "equipment",
                Budget = 200
            },
            new()
            {
                Key = "material",
                Budget = 50
            },
            new()
            {
                Key = "other",
                Budget = 90
            }
        };

        return Task.FromResult(dummyData);
    }
}