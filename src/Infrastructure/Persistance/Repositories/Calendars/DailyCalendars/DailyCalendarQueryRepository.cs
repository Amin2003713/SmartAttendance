using SmartAttendance.Application.Features.Calendars.Request.Commands.CreateReminder;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetHoliday;
using SmartAttendance.Application.Features.Calendars.Request.Queries.GetReminder;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Domain.Calenders.DailyCalender;

namespace SmartAttendance.Persistence.Repositories.Calendars.DailyCalendars;

public class DailyCalendarQueryRepository(
    ReadOnlyDbContext dbContext,
    ICalendarQueryRepository calendarQueryRepository,
    ILogger<DailyCalendarQueryRepository> logger,
    IStringLocalizer<DailyCalendarQueryRepository> localizer
)
    : QueryRepository<DailyCalendar>(dbContext, logger),
        IDailyCalendarQueryRepository
{
    public async Task<bool> IsAlreadyHoliday(DateTime dateTime, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if the date {Date} is already a holiday",
                dateTime);

            var isAlreadyHoliday = await TableNoTracking.AnyAsync(
                c => c.Date == dateTime &&
                     c.IsHoliday &&
                     c.DeletedBy == null,
                cancellationToken);

            var holiday = await calendarQueryRepository.IsAlreadyHoliday(dateTime, cancellationToken);
            logger.LogInformation("Holiday check result: {Result}", isAlreadyHoliday || holiday);

            return isAlreadyHoliday || holiday;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while checking if the date is already");

            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while checking holiday status."]);
        }
    }


    public async Task<List<GetHolidayResponse>> GetHolidaysForMonth(
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching holidays  between {StartAt} and {EndAt}",
                startAt,
                endAt);

            var privateHolidays = await TableNoTracking.Where(e => e.Date >= startAt &&
                                                                   e.Date <= endAt &&
                                                                   e.IsHoliday &&
                                                                   e.DeletedBy == null)
                .Select(e =>
                    new GetHolidayResponse
                    {
                        Date = e.Date,
                        Message = e.Details,
                        IsCustom = true,
                        Id = e.Id,
                        Inserter = DbContext.Set<User>()
                            .Where(u => u.Id == e.CreatedBy)
                            .Select(u => new
                            {
                                u.Id,
                                Name = u.FirstName + " " + u.LastName
                            })
                            .FirstOrDefault()!
                    })
                .ToListAsync(cancellationToken);

            var publicHolidays =
                await calendarQueryRepository.GetHolidaysForMonth(startAt, endAt, cancellationToken);

            var holiday = privateHolidays.Concat(publicHolidays).OrderBy(h => h.Date).ToList();

            return holiday;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching holidays");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while fetching holidays for project."]);
        }
    }


    public async Task<List<GetReminderResponse>> GetReminderForProject(
        Guid userId,
        DateTime startAt,
        DateTime endAt,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Fetching reminders for user {UserId} between {StartAt} and {EndAt}",
                userId,
                startAt,
                endAt);

            var privateReminders = await TableNoTracking.Where(e => e.Date >= startAt &&
                                                                    e.Date <= endAt &&
                                                                    e.CalendarUsers.Any(cu => cu.UserId == userId) &&
                                                                    e.IsReminder &&
                                                                    e.DeletedBy == null)
                .OrderBy(a => a.Date)
                .Select(e =>
                    new GetReminderResponse
                    {
                        Id = e.Id,
                        Date = e.Date,
                        Message = e.Details,
                        Inserter = DbContext.Set<User>()
                            .Where(u => u.Id == e.CreatedBy)
                            .Select(u => new
                            {
                                u.Id,
                                Name = u.FirstName + " " + u.LastName
                            })
                            .FirstOrDefault()!,
                        TargetUsers = e.CalendarUsers.Where(cu => cu.DeletedBy == null)
                            .Select(cu =>
                                new UserTargetRequest
                                {
                                    Id = cu.UserId,
                                    Name = DbContext.Set<User>().First(u => u.Id == cu.UserId).FullName()
                                })
                            .ToList()
                    })
                .ToListAsync(cancellationToken);

            return privateReminders;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching reminders ");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while fetching reminders for project."]);
        }
    }


    public async Task<List<DailyCalendar>?> GetCustomCalendarEvents(
        DateTime startDate,
        DateTime endDate,
        Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching custom holidays for date: {start} to {end}",
                startDate,
                endDate);

            var response = await TableNoTracking.Where(a => a.Date <= endDate &&
                                                            a.Date >= startDate &&
                                                            (a.IsReminder || a.IsMeeting || a.IsHoliday) &&
                                                            (
                                                                !a.IsReminder ||
                                                                a.CalendarUsers.Any(cu => cu.UserId == userId)) &&
                                                            a.DeletedBy == null)
                .ToListAsync(cancellationToken);

            logger.LogInformation("Custom holiday found: {Found}", response != null);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while fetching custom holiday for date: {Date} ",
                endDate);

            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while fetching custom holiday."]);
        }
    }

    public async Task<DailyCalendar?> Getday(Guid Id, CancellationToken cancellationToken)
    {
        try
        {
            return await TableNoTracking.FirstOrDefaultAsync(a => a.Id == Id, cancellationToken) ?? null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}