using Shifty.Application.Features.Dashboards.Queries.BudgetDistributionCharts;
using Shifty.Application.Features.Dashboards.Queries.KPIMetric;
using Shifty.Application.Features.Dashboards.Requests.ProgressHistory;
using Shifty.Application.Features.Dashboards.Responses.DashboardCardSummary;
using Shifty.Application.Features.Dashboards.Responses.ProgressHistory;
using Shifty.Application.Features.Dashboards.Responses.ResourceChartDat;

namespace Shifty.Api.Controllers.Dashboards;

public class DashboardController : ShiftyBaseController
{
    // /// <summary>
    // ///     Retrieves recent entity activities (created/updated)
    // /// </summary>
    // /// <param name="request">Filter options such as days back and max records</param>
    // /// <param name="cancellationToken">Cancellation token</param>
    // /// <returns>List of recent activity messages</returns>
    // [HttpGet("recent-activities")]
    // [SwaggerOperation(Summary = "Get Recent Activities",
    //     Description = "Returns a list of recent create/update actions.")]
    // [ProducesResponseType(typeof(List<GetResentActivities>), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ApiProblemDetails),         StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(ApiProblemDetails),         StatusCodes.Status401Unauthorized)]
    // public async Task<List<GetResentActivities>> GetRecentActivities(
    //     [FromQuery] GetRecentActivitiesRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     return await Mediator.Send(request.Adapt<GetRecentActivitiesQuery>(), cancellationToken);
    // }

    /// <summary>
    ///     Returns historical monthly progress data aggregated across all company projects.
    /// </summary>
    /// <param name="request">Optional filtering parameters (e.g. specific projectId for future extension)</param>
    /// <returns>List of monthly progress values representing actual vs planned percentages</returns>
    [HttpGet("progress-history")]
    [SwaggerOperation(
        Summary = "Get Projects Progress History (Collection of all the Projects)",
        Description
            = """
              Returns company-wide monthly actual vs planned progress default set times interval is from the start of the company until now .
              Each item represents the average progress across all projects in that month.
              """
    )]
    [ProducesResponseType(typeof(List<ProjectProgressHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),                    StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),                    StatusCodes.Status401Unauthorized)]
    public Task<List<ProjectProgressHistoryResponse>> GetProjectProgressHistoryHistory([FromQuery] ProjectProgressHistoryRequest request)
    {
        var results = new List<ProjectProgressHistoryResponse>();
        var rand    = new Random();
        var now     = DateTime.UtcNow;
        var years   = now.Date.AddYears(-3);
        var inter   = (now - years).Days;


        for (var i = 0; i <= inter; i++) // 3 years of monthly progress
        {
            var monthDate = years.AddDays(i);

            results.Add(new ProjectProgressHistoryResponse
            {
                Date = monthDate,
                Actual = rand.Next(0,  101), // Simulated average actual progress
                Planned = rand.Next(0, 101)  // Simulated average planned progress
            });
        }

        return Task.FromResult(results);
    }

    /// <summary>
    ///     Returns dummy chart data showing resource budget distribution
    /// </summary>
    /// <returns>List of key-budget pairs for chart visualization</returns>
    [HttpGet("Budget-distribution-chart")]
    [SwaggerOperation(Summary = "Get Resource Budget Chart Data",
        Description = "Returns sample resource distribution for chart display.")]
    [ProducesResponseType(typeof(List<BudgetDistributionChartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),                     StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),                     StatusCodes.Status401Unauthorized)]
    public async Task<List<BudgetDistributionChartResponse>> GetBudgetDistributionChart(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new BudgetDistributionChartQuery(), cancellationToken);
    }

    /// <summary>
    ///     Get high-level summary KPIs for the main dashboard
    /// </summary>
    /// <returns>Single summary object with progress, delay, budget, and active projects</returns>
    [HttpGet("KPI-metrics")]
    [SwaggerOperation(Summary = "Get Dashboard Summary KPIs",
        Description = "Returns metrics like progress %, delay, budget usage, and active project count.")]
    [ProducesResponseType(typeof(List<KPIMetricsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),        StatusCodes.Status401Unauthorized)]
    public async Task<List<KPIMetricsResponse>> GetDashboardSummary(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new KPIMetricsQuery(), cancellationToken);
    }
}