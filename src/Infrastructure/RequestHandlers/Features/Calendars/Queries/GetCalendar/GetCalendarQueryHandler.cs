using DNTPersianUtils.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Features.Calendars.Queries.GetCalendar;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetCalendar;
using SmartAttendance.Application.Features.Plans.Queries.GetByDate;
using SmartAttendance.Application.Features.Plans.Responses;
using SmartAttendance.Application.Interfaces.Plans;
using SmartAttendance.Application.Interfaces.Tenants.Calendars;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Queries.GetCalendar;

public class GetCalendarQueryHandler(
    IdentityService                  identityService,
    IMediator mediator,
    ICalendarQueryRepository         calendarQueryRepository,
    ILogger<GetCalendarQueryHandler> logger
) : IRequestHandler<GetCalendarQuery, List<GetCalendarResponse>>
{
    public async Task<List<GetCalendarResponse>> Handle(
        GetCalendarQuery  request,
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
                                            calendar.Date <= monthEndGregorian   &&
                                            calendar.IsActive                    &&
                                            (calendar.IsHoliday ||
                                             calendar.IsWeekend ||
                                             calendar.Details != null),
                                        cancellationToken) ??
                                    [];


        var userPlans = (await mediator.Send(new GetPlanByDateRangeQuery
                {
                    From = monthStartGregorian,
                    To = monthEndGregorian
                },
                cancellationToken))
            .GroupBy(a => a.StartTime.Date)
            .ToDictionary(g => g.Key, g => g.ToList());


        var dates = userPlans.Select(a => a.Key).Intersect(publicCalendarEntries.Select(a => a.Key)).ToList();

        var calendarResponses = new List<GetCalendarResponse>(dates.Count);

        foreach (var date in dates.OrderBy(d => d))
        {
            publicCalendarEntries.TryGetValue(date, out var publicEntry);

            var isPublicHolidayOrWeekend = IsPublicHolidayOrWeekend(publicEntry ?? []);

            userPlans.TryGetValue(date, out var customEntry);


            if (!isPublicHolidayOrWeekend &&
                customEntry is { Count: 0 })
                continue;


            calendarResponses.Add(new GetCalendarResponse
            {
                Date            = date,
                IsHoliday       = isPublicHolidayOrWeekend,
                PlanInfos = customEntry ?? []
            });
        }

        logger.LogInformation(
            "Calendar built with {Count} days ",
            calendarResponses.Count);

        return calendarResponses;
    }


    private bool IsPublicHolidayOrWeekend(List<TenantCalendar> publicEntries)
    {
        return publicEntries is not { Count: 0 } && publicEntries.Any(a => a.IsHoliday || a.IsWeekend);
    }
}