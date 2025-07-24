using Shifty.Application.Dashboards.Queries.BudgetDistributionCharts;
using Shifty.Application.Dashboards.Responses.ResourceChartDat;

namespace Shifty.RequestHandlers.Dashboards.Queries.BudgetDistributionCharts;

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
                Key = "humanResource", Budget = 275
            },
            new()
            {
                Key = "equipment", Budget = 200
            },
            new()
            {
                Key = "material", Budget = 50
            },
            new()
            {
                Key = "other", Budget = 90
            }
        };

        return Task.FromResult(dummyData);
    }
}