using DNTPersianUtils.Core;
using Shifty.Application.Calendars.Queries.GetCalendar;
using Shifty.Application.Calendars.Request.Queries.GetCalendar;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Application.Interfaces.Tenants.Calendars;
using Shifty.Domain.Calenders.DailyCalender;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Calendars.Queries.GetCalendar;

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
            "Fetching calendar for ProjectId:  Year: {Year}, Month: {Month}, User: {UserId}",
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


        var publicByDate = publicCalendarEntries.GroupBy(e => e.Date.Date).ToDictionary(g => g.Key, g => g.First());

        var customByDate = customCalendarEntries.GroupBy(h => h.Date.Date).ToDictionary(g => g.Key, g => g.First());


        var datesWithPublicEvent = publicByDate.Keys;
        var datesWithCustomEntry = customByDate.Keys;

        var candidateDates = new HashSet<DateTime>(datesWithPublicEvent);
        candidateDates.UnionWith(datesWithCustomEntry);

        // var reviewStatusTasks
        //     = await calendarQueryRepository.GetReviewStatusBetweenDateAsync(monthStartGregorian,
        //         monthEndGregorian,
        //         request.ProjectId,
        //         access,
        //         cancellationToken);
        //
        //
        // var datesWithReviewStatus = reviewStatusTasks.Where(kvp =>
        //     {
        //         var (actionNeeded, isVerified) = kvp.Value;
        //         return actionNeeded || isVerified;
        //     })
        //     .Select(kvp => kvp.Key)
        //     .ToHashSet();

        var interestingDates = new HashSet<DateTime>(datesWithPublicEvent);
        interestingDates.UnionWith(datesWithCustomEntry);
        // interestingDates.UnionWith(datesWithReviewStatus);


        var calendarResponses = new List<GetCalendarResponse>(interestingDates.Count);

        foreach (var date in interestingDates.OrderBy(d => d))
        {
            publicByDate.TryGetValue(date, out var publicEntry);

            var isPublicHolidayOrWeekend = IsPublicHolidayOrWeekend(publicEntry);

            customByDate.TryGetValue(date, out var customEntry);

            var isCustomHoliday = IsCustomHoliday(customEntry);

            var hasReminder = IsReminder(customEntry);
            //
            // reviewStatusTasks.TryGetValue(date, out var state);
            //
            // state ??= new FetchReviewStatusResult(false, false);

            if (!isPublicHolidayOrWeekend &&
                !isCustomHoliday &&
                !hasReminder
                // && state is { ActionNeeded: false, IsVerified: false }
               )
                continue;


            calendarResponses.Add(new GetCalendarResponse
            {
                Date = date,
                IsHoliday = isPublicHolidayOrWeekend,
                IsCustomHoliday = isCustomHoliday,
                HasReminder = hasReminder
                // ActionNeeded = state.ActionNeeded,
                // IsVerified = state.IsVerified
            });
        }

        // logger.LogInformation(
        //     "Calendar built with {Count} days for ProjectId: {ProjectId}",
        //     calendarResponses.Count,
        //     request.ProjectId);

        return calendarResponses;
    }


    /// <summary>
    ///     Returns true if the given public‐calendar entry represents a weekend or a holiday.
    /// </summary>
    private bool IsPublicHolidayOrWeekend(TenantCalendar? publicEntry)
    {
        if (publicEntry == null) return false;

        var isDayOff = publicEntry.IsHoliday || publicEntry.IsWeekend;

        logger.LogTrace("Checked IsPublicHolidayOrWeekend for {Date}: {IsDayOff}",
            publicEntry.Date,
            isDayOff);

        return isDayOff;
    }

    /// <summary>
    ///     Returns true if the given daily‐calendar entry is marked as a holiday.
    /// </summary>
    private bool IsCustomHoliday(DailyCalendar? dailyEntry)
    {
        return dailyEntry is { IsHoliday: true };
    }

    /// <summary>
    ///     Returns true if the given daily‐calendar entry is marked as a reminder.
    /// </summary>
    private bool IsReminder(DailyCalendar? dailyEntry)
    {
        return dailyEntry is { IsReminder: true };
    }
}