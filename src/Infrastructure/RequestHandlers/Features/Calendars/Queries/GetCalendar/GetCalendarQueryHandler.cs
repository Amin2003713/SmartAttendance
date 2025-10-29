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
    IdentityService identityService,
    IMediator mediator,
    ICalendarQueryRepository calendarQueryRepository,
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
            new PersianDateTime(request.Year, request.Month, 1)
                .ToString()
                .ToGregorianDateTime()!.Value;

        var monthEndGregorian = monthStartGregorian
            .GetPersianMonthStartAndEndDates()!.EndDate;

        var publicCalendarEntries =
            await calendarQueryRepository.GetPublicCalendarEvents(
                c => c.Date >= monthStartGregorian &&
                     c.Date <= monthEndGregorian &&
                     c.IsActive &&
                     (c.IsHoliday || c.IsWeekend || c.Details != null),
                cancellationToken
            ) ??
            [];

        var publicCalendarByDate = publicCalendarEntries
            .GroupBy(c => c.Key)
            .ToDictionary(g => g.Key, g => g.ToList());

        var userPlans = (await mediator.Send(
                new GetPlanByDateRangeQuery
                {
                    From = monthStartGregorian,
                    To = monthEndGregorian
                },
                cancellationToken))
            .GroupBy(a => a.StartTime.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        var allDates = userPlans.Keys
            .Union(publicCalendarByDate.Keys)
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        var calendarResponses = new List<GetCalendarResponse>(allDates.Count);

        foreach (var date in allDates)
        {
            publicCalendarByDate.TryGetValue(date, out var publicEntry);
            userPlans.TryGetValue(date, out var planEntry);

            var isHolidayOrWeekend = publicEntry?.Any(c => c.Value.Any(a => a.IsHoliday) || c.Value.Any(a => a.IsWeekend)) ?? false;

            calendarResponses.Add(new GetCalendarResponse
            {
                Date = date,
                IsHoliday = isHolidayOrWeekend,
                PlanInfos = planEntry?.Adapt<List<GetPlanInfoCalendarResponse>>() ?? []    ,
                Details = publicEntry!.SelectMany(a => a.Value.Select(a => a.Details)) .ToList()
            });
        }

        logger.LogInformation("Calendar built with {Count} days", calendarResponses.Count);

        return calendarResponses;
    }
}