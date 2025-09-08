using DNTPersianUtils.Core;
using SmartAttendance.Application.Features.Calendars.Queries.GetHoliday;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetHoliday;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Queries.GetHoliday;

public class GetHolidayQueryHandler(
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    ILogger<GetHolidayQueryHandler> logger
)
    : IRequestHandler<GetHolidayQuery, List<GetHolidayResponse>>
{
    public async Task<List<GetHolidayResponse>> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var fromDate = new PersianDateTime(request.Year, request.Month, 1).ToString().ToGregorianDateTime();

            var monthRange = fromDate.GetPersianMonthStartAndEndDates();
            var toDate     = monthRange!.EndDate;

            logger.LogInformation("Fetching holidays from {Start} to {End}",
                fromDate,
                toDate);

            var holidays =
                await dailyCalendarQueryRepository.GetHolidaysForMonth(
                    fromDate.Value,
                    toDate,
                    cancellationToken);

            logger.LogInformation("Found {Count} holidays ",
                holidays.Count);

            return holidays;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while fetching holidays  Year: {Year}, Month: {Month}",
                request.Year,
                request.Month);

            throw SmartAttendanceException.InternalServerError();
        }
    }
}