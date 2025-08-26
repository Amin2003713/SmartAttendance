namespace Shifty.Application.Features.Dashboards.Requests.ProgressHistory;

public record ProjectProgressHistoryRequest
{
    public int DaysBack { get; set; }
}