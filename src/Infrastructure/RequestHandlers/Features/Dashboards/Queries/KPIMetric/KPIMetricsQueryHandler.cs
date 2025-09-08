using SmartAttendance.Application.Features.Dashboards.Queries.KPIMetric;
using SmartAttendance.Application.Features.Dashboards.Responses.DashboardCardSummary;
using SmartAttendance.Common.General.Enums.Dashboard;

namespace SmartAttendance.RequestHandlers.Features.Dashboards.Queries.KPIMetric;

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