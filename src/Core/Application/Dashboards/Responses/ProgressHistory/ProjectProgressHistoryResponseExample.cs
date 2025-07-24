using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Dashboards.Responses.ProgressHistory;

public class ProjectProgressHistoryResponseExample : IExamplesProvider<List<ProjectProgressHistoryResponse>>
{
    public List<ProjectProgressHistoryResponse> GetExamples()
    {
        return
        [
            new ProjectProgressHistoryResponse
            {
                Date = DateTime.Now, Actual = 10, Planned = 23
            },
            new ProjectProgressHistoryResponse
            {
                Date = DateTime.Now.AddDays(-1), Actual = 9, Planned = 20
            }
        ];
    }
}