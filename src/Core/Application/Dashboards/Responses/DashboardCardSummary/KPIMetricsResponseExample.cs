using Shifty.Common.General.Enums.Dashboard;

namespace Shifty.Application.Dashboards.Responses.DashboardCardSummary;

public class KPIMetricsResponseExample : IExamplesProvider<List<KPIMetricsResponse>>
{
    public List<KPIMetricsResponse> GetExamples()
    {
        return
        [
            new KPIMetricsResponse
            {
                Title = "پیشرفت کلی", // Overall Progress
                Value = 68,
                Growth = 2,
                GrowthType = GrowthType.Expanded,
                Type = KPIType.OverallProgress
            },

            new KPIMetricsResponse
            {
                Title = "تأخیرهای فعلی", // Current Delay
                Value = 7,
                Growth = 0,
                GrowthType = GrowthType.Stable,
                Type = KPIType.CurrentDelay
            },

            new KPIMetricsResponse
            {
                Title = "بودجه مصرف شده", // Budget Consumed
                Value = 82,
                Growth = 5,
                GrowthType = GrowthType.Shrink,
                Type = KPIType.BudgetConsumed
            },

            new KPIMetricsResponse
            {
                Title = "پروژه‌های فعال", // Active Projects
                Value = 12,
                Growth = 2,
                GrowthType = GrowthType.Expanded,
                Type = KPIType.ActiveProjects
            }
        ];
    }
}