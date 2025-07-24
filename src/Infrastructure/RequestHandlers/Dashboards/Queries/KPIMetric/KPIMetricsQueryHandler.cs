using Shifty.Application.Dashboards.Queries.KPIMetric;
using Shifty.Application.Dashboards.Responses.DashboardCardSummary;
using Shifty.Common.General.Enums.Dashboard;

namespace Shifty.RequestHandlers.Dashboards.Queries.KPIMetric;

public class KPIMetricsQueryHandler : IRequestHandler<KPIMetricsQuery, List<KPIMetricsResponse>>
{
    public Task<List<KPIMetricsResponse>> Handle(KPIMetricsQuery request, CancellationToken cancellationToken)
    {
        var cardSummaries = new List<KPIMetricsResponse>
        {
            new()
            {
                Title = "پیشرفت کلی", // Overall Progress
                Value = 68,
                Growth = 2,
                GrowthType = GrowthType.Expanded,
                Type = KPIType.OverallProgress
            },
            new()
            {
                Title = "تأخیرهای فعلی", // Current Delay
                Value = 7,
                Growth = 0,
                GrowthType = GrowthType.Stable,
                Type = KPIType.CurrentDelay
            },
            new()
            {
                Title = "بودجه مصرف شده", // Budget Consumed
                Value = 82,
                Growth = 5,
                GrowthType = GrowthType.Shrink,
                Type = KPIType.BudgetConsumed
            },
            new()
            {
                Title = "پروژه‌های فعال", // Active Projects
                Value = 12,
                Growth = 2,
                GrowthType = GrowthType.Expanded,
                Type = KPIType.ActiveProjects
            }
        };

        return Task.FromResult(cardSummaries);
    }
}