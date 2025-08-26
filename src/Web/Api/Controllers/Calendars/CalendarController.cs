using Shifty.Application.Base.Calendars.Commands.CreateHoliday;
using Shifty.Application.Base.Calendars.Commands.CreateReminder;
using Shifty.Application.Base.Calendars.Commands.DeleteHoliday;
using Shifty.Application.Base.Calendars.Commands.DeleteReminder;
using Shifty.Application.Base.Calendars.Commands.UpdateHoliday;
using Shifty.Application.Base.Calendars.Commands.UpdateReminder;
using Shifty.Application.Base.Calendars.Queries.GetCalendar;
using Shifty.Application.Base.Calendars.Queries.GetCalendarInfo;
using Shifty.Application.Base.Calendars.Queries.GetCalendarLevelReport;
using Shifty.Application.Base.Calendars.Queries.GetHoliday;
using Shifty.Application.Base.Calendars.Queries.GetReminder;
using Shifty.Application.Base.Calendars.Request.Commands.CreateHoliday;
using Shifty.Application.Base.Calendars.Request.Commands.CreateReminder;
using Shifty.Application.Base.Calendars.Request.Commands.UpdateHoliday;
using Shifty.Application.Base.Calendars.Request.Commands.UpdateReminder;
using Shifty.Application.Base.Calendars.Request.Queries.GetCalendar;
using Shifty.Application.Base.Calendars.Request.Queries.GetCalendarLevelReport;
using Shifty.Application.Base.Calendars.Request.Queries.GetHoliday;
using Shifty.Application.Base.Calendars.Request.Queries.GetReminder;
using GetCalendarInfoResponse = Shifty.Common.Common.Responses.FeatureCalendarResponse.GetCalendarInfoResponse;

namespace Shifty.Api.Controllers.Calendars;

public class CalendarController : IpaBaseController
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

        int year,
        int month,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCalendarQuery( year, month), cancellationToken);
    }


    /// <summary>
    ///     Gets detailed information for a specific day in the project calendar.
    /// </summary>
    /// <param name="projectId">Projects identifier.</param>
    /// <param name="date">Target date to retrieve calendar info.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Calendar day info retrieved successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("Get-Calendar-Info-For-Day")]
    [ProducesResponseType(typeof(GetCalendarInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetCalendarInfoResponse>> GetCalendarInfoForDay(

        DateTime date,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCalendarInfoQuery( date), cancellationToken);
    }


    /// <summary>
    ///     Gets calendar activity level report for a specific day.
    /// </summary>
    /// <param name="projectId">Projects identifier.</param>
    /// <param name="date">Target date to retrieve level report.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Calendar level report retrieved successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("Get-Calendar-Level-Report")]
    [ProducesResponseType(typeof(List<CalendarInfoForLevelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<CalendarInfoForLevelDto>> GetCalendarInfo(

        DateTime date,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetCalendarLevelReportQuery( date), cancellationToken);
    }


    /// <summary>
    ///     Adds a new reminder to the project calendar.
    /// </summary>
    /// <param name="request">Requests containing reminder details.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">Reminder added successfully.</response>
    [HttpPost("Add-Reminder")]
    [SwaggerOperation("Add-Reminder")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProjectAccess((int)TenantAccess.ReminderAccess)]
    public async Task AddReminderToCalendar(
        [FromBody] CreateReminderRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateReminderCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Adds a new holiday to the project calendar.
    /// </summary>
    /// <param name="request">Requests containing holiday details.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">Holiday added successfully.</response>
    [HttpPost]
    [SwaggerOperation("Add-Holiday")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task AddHolidayToCalendar(
        [FromBody] CreateHolidayRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<CreateHolidayCommand>(), cancellationToken);
    }


    /// <summary>
    ///     Retrieves holidays for a specific month of the project calendar.
    /// </summary>
    /// <param name="projectId">Projects identifier.</param>
    /// <param name="year">The year to filter holidays.</param>
    /// <param name="month">The month to filter holidays.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Holidays retrieved successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("Get-Holidays")]
    [ProducesResponseType(typeof(List<GetHolidayResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetHolidayResponse>> GetHolidays(

        int year,
        int month,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetHolidayQuery( year, month), cancellationToken);
    }

    /// <summary>
    ///     Retrieves reminders for a specific month of the project calendar.
    /// </summary>
    /// <param name="projectId">Projects identifier.</param>
    /// <param name="year">The year to filter reminders.</param>
    /// <param name="month">The month to filter reminders.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Reminders retrieved successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpGet("Get-Reminders")]
    [ProducesResponseType(typeof(List<GetReminderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<List<GetReminderResponse>> GetReminder(

        int year,
        int month,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetReminderQuery(year, month), cancellationToken);
    }

    /// <summary>
    ///     Updates an existing reminder in the calendar.
    /// </summary>
    /// <param name="request">Requests containing updated reminder data.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">Reminder updated successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpPut("Update-Reminder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task UpdateReminder(
        [FromBody] UpdateReminderRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateReminderCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Updates an existing holiday in the calendar.
    /// </summary>
    /// <param name="request">Requests containing updated holiday data.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">Holiday updated successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpPut("Update-Holiday")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProjectAccess((int)TenantAccess.HolidayAccess)]
    public async Task UpdateHoliday(
        [FromBody] UpdateHolidayRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateHolidayCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Deletes a reminder from the calendar by its ID.
    /// </summary>
    /// <param name="ReminderId">Identifier of the reminder to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">Reminder deleted successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpDelete("Delete-Reminder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task DeleteReminder(Guid ReminderId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteReminderCommand(ReminderId), cancellationToken);
    }


    /// <summary>
    ///     Deletes a holiday from the calendar by its ID.
    /// </summary>
    /// <param name="HolidayId">Identifier of the holiday to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">Holiday deleted successfully.</response>
    /// <response code="400">Invalid request.</response>
    /// <response code="401">Unauthorized access.</response>
    [HttpDelete("Delete-Holiday")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task DeleteHoliday(Guid HolidayId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteHolidayCommand(HolidayId), cancellationToken);
    }
}