namespace Shifty.Application.Features.Dashboards.Requests.GetRecentActivities;

public class GetRecentActivitiesRequest
{
    public int DaysBack { get; set; } = 30;
    public int TopNRecords { get; set; } = 10;

    // public Guid? ProjectId { get; set; } = null!;
}