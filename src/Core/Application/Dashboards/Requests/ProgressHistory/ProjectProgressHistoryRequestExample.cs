namespace Shifty.Application.Dashboards.Requests.ProgressHistory;

public record ProjectProgressHistoryRequestExample : IExamplesProvider<ProjectProgressHistoryRequest>
{
    public ProjectProgressHistoryRequest GetExamples()
    {
        return new ProjectProgressHistoryRequest
        {
            DaysBack = 30
        };
    }
}