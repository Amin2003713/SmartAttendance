namespace Shifty.Application.Features.Dashboards.Responses.ProgressHistory;

public class ProjectProgressHistoryResponse
{
    /// <summary>
    ///     First day of the month representing the period of the progress
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    ///     Aggregated actual progress percentage across all projects
    /// </summary>
    public int Actual { get; set; }

    /// <summary>
    ///     Aggregated planned progress percentage across all projects
    /// </summary>
    public int Planned { get; set; }
}