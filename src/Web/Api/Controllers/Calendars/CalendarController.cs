
using SmartAttendance.Application.Features.Calendars.Queries.GetCalendar;

using SmartAttendance.Application.Features.Calendars.Request.Queries.GetCalendar;


namespace SmartAttendance.Api.Controllers.Calendars;

public class CalendarController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Gets the calendar data for a specific project and month.
    /// </summary>
    /// <param name="projectId">Projects identifier.</param>
    /// <param name="year">The year of the calendar.</param>
    /// <param name="month">The month of the calendar.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Calendar data retrieved successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<GetCalendarResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetCalendarResponse>> GetCalendar(
        int               year,
        int               month,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCalendarQuery(year, month), cancellationToken);
    }
}