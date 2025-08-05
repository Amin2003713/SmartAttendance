namespace Shifty.Application.Dashboards.Responses.ProgressHistory;

public class ProjectProgressHistoryResponseExample : IExamplesProvider<List<ProjectProgressHistoryResponse>>
{
    public List<ProjectProgressHistoryResponse> GetExamples()
    {
        return
        [
            new ProjectProgressHistoryResponse
            {
                Date = DateTime.UtcNow,
                Actual = 10,
                Planned = 23
            },
            new ProjectProgressHistoryResponse
            {
                Date = DateTime.UtcNow.AddDays(-1),
                Actual = 9,
                Planned = 20
            }
        ];
    }
}