using DNTPersianUtils.Core;
using Shifty.Application.Features.Calendars.Queries.GetCalendar;
using Shifty.Application.Features.Calendars.Request.Queries.GetCalendar;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Application.Interfaces.Tenants.Calendars;
using Shifty.Domain.Calenders.DailyCalender;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Calendars.Queries.GetCalendar;

public class GetCalendarQueryHandler(
    IdentityService identityService,
    ICalendarQueryRepository calendarQueryRepository,
    IMediator mediator,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    ILogger<GetCalendarQueryHandler> logger
) : IRequestHandler<GetCalendarQuery, List<GetCalendarResponse>>
{
    public async Task<List<GetCalendarResponse>> Handle(
        GetCalendarQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = identityService.GetUserId<Guid>();

        logger.LogInformation(
            "Fetching calendar Year: {Year}, Month: {Month}, User: {UserId}",
            request.Year,
            request.Month,
            currentUserId);

        var monthStartGregorian =
            new PersianDateTime(request.Year, request.Month, 1).ToString().ToGregorianDateTime()!.Value;

        var monthEndGregorian = monthStartGregorian.GetPersianMonthStartAndEndDates()!.EndDate;

        var publicCalendarEntries = await calendarQueryRepository.GetPublicCalendarEvents(calendar =>
                                            calendar.Date >= monthStartGregorian &&
                                            calendar.Date <= monthEndGregorian &&
                                            calendar.IsActive &&
                                            (calendar.IsHoliday ||
                                             calendar.IsWeekend ||
                                             calendar.Details != null),
                                        cancellationToken) ??
                                    [];


        var customCalendarEntries = await dailyCalendarQueryRepository.GetCustomCalendarEvents(
                                        monthStartGregorian,
                                        monthEndGregorian,
                                        currentUserId,
                                        cancellationToken) ??
                                    [];


        var publicByDate = publicCalendarEntries.GroupBy(e => e.Date.Date)
            .ToDictionary(g => g.Key, g => g.First());

        var customByDate = customCalendarEntries.GroupBy(h => h.Date.Date)
            .ToDictionary(g => g.Key,
                g =>
                {
                    var merged = new DailyCalendar
                    {
                        Date = g.Key,
                        IsReminder = g.Any(x => x.IsReminder),
                        IsHoliday = g.Any(x => x.IsHoliday),
                        IsMeeting = g.Any(x => x.IsMeeting),
                        Details = g.Select(x => x.Details).FirstOrDefault(d => !string.IsNullOrWhiteSpace(d)),
                        CalendarUsers = []
                    };

                    return merged;
                });


        var datesWithPublicEvent = publicByDate.Keys;
        var datesWithCustomEntry = customByDate.Keys;

        var interestingDates = new HashSet<DateTime>(datesWithPublicEvent);
        interestingDates.UnionWith(datesWithCustomEntry);


        var calendarResponses = new List<GetCalendarResponse>(interestingDates.Count);

        foreach (var date in interestingDates.OrderBy(d => d))
        {
            publicByDate.TryGetValue(date, out var publicEntry);

            var isPublicHolidayOrWeekend = IsPublicHolidayOrWeekend(publicEntry);

            customByDate.TryGetValue(date, out var customEntry);

            var isCustomHoliday = IsCustomHoliday(customEntry);

            var hasReminder = IsReminder(customEntry);

            if (!isPublicHolidayOrWeekend &&
                !isCustomHoliday &&
                !hasReminder)
                continue;


            calendarResponses.Add(new GetCalendarResponse
            {
                Date = date,
                IsHoliday = isPublicHolidayOrWeekend,
                IsCustomHoliday = isCustomHoliday,
                HasReminder = hasReminder
            });
        }

        logger.LogInformation(
            "Calendar built with {Count} days ",
            calendarResponses.Count);

        return calendarResponses;
    }

    private bool IsPublicHolidayOrWeekend(TenantCalendar? publicEntry)
    {
        if (publicEntry == null) return false;

        var isDayOff = publicEntry.IsHoliday || publicEntry.IsWeekend;

        logger.LogTrace("Checked IsPublicHolidayOrWeekend for {Date}: {IsDayOff}",
            publicEntry.Date,
            isDayOff);

        return isDayOff;
    }

    private static bool IsCustomHoliday(DailyCalendar? dailyEntry)
    {
        return dailyEntry is { IsHoliday: true };
    }

    private static bool IsReminder(DailyCalendar? dailyEntry)
    {
        return dailyEntry is { IsReminder: true };
    }
}