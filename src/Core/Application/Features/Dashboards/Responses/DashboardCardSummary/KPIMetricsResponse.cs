using SmartAttendance.Common.General.Enums.Dashboard;

namespace SmartAttendance.Application.Features.Dashboards.Responses.DashboardCardSummary;

public class KPIMetricsResponse
{
    public string Title { get; set; } = null!;
    public byte Value { get; set; }
    public byte Growth { get; set; }
    public GrowthType GrowthType { get; set; }

    public KPIType Type { get; set; }
}