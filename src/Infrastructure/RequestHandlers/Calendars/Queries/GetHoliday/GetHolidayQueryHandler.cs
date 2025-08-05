using DNTPersianUtils.Core;
using Shifty.Application.Calendars.Queries.GetHoliday;
using Shifty.Application.Calendars.Request.Queries.GetHoliday;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Calendars.Queries.GetHoliday;

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
            var fromDate = new PersianDateTime(request.year, request.month, 1).ToString().ToGregorianDateTime();

            var monthRange = fromDate.GetPersianMonthStartAndEndDates();
            var toDate     = monthRange!.EndDate;

            // logger.LogInformation("Fetching holidays for ProjectId: {ProjectId} from {Start} to {End}",
            //     request.projectId,
            //     fromDate,
            //     toDate);

            var holidays =
                await dailyCalendarQueryRepository.GetHolidaysForMonth(
                    fromDate!.Value,
                    toDate,
                    cancellationToken);

            // logger.LogInformation("Found {Count} holidays for ProjectId: {ProjectId}",
            //     holidays.Count,
            //     request.projectId);

            return holidays;
        }
        catch (Exception ex)
        {
            // logger.LogError(ex,
            //     "Error occurred while fetching holidays for ProjectId: {ProjectId}, Year: {Year}, Month: {Month}",
            //     request.projectId,
            //     request.year,
            //     request.month);

            throw IpaException.InternalServerError();
        }
    }
}