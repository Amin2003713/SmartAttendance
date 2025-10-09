using SmartAttendance.Application.Features.Plans.Commands.Create;
using SmartAttendance.Application.Features.Plans.Queries.GetByDate;
using SmartAttendance.Application.Features.Plans.Responses;

namespace SmartAttendance.Api.Controllers.Plans;

public class PlanController : SmartAttendanceBaseController
{
    /// <summary>
    /// Creates a new plan with subjects and teachers.
    /// </summary>
    /// <param name="request">Plan creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Plan created successfully.</response>
    /// <response code="400">Invalid request or missing references.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task CreatePlan([FromBody] CreatePlanCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
    }

    /// <summary>
    /// Gets plans for a specific date range filtered by user roles.
    /// </summary>
    /// <param name="from">Start date.</param>
    /// <param name="to">End date.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Plans retrieved successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("by-date-range")]
    [ProducesResponseType(typeof(List<GetPlanInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetPlanInfoResponse>> GetPlansByDateRange(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetPlanByDateRangeQuery
            {
                From = from,
                To = to
            },
            cancellationToken);
    }
}