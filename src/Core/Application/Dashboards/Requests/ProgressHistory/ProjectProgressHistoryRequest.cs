namespace Shifty.Application.Dashboards.Requests.ProgressHistory;

public record ProjectProgressHistoryRequest
{
    public int DaysBack { get; set; }
}