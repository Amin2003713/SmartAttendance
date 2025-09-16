using Mapster;
using SmartAttendance.Application.Features.Calendars.Commands.CreateReminder;
using SmartAttendance.Application.Interfaces.Calendars.CalendarUsers;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Calenders.CalenderUsers;
using SmartAttendance.Domain.Calenders.DailyCalender;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Commands.CreateReminder;

public class CreateReminderCommandHandler(
    IdentityService                                service,
    IDailyCalendarCommandRepository                dailyCalendarCommandRepository,
    ICalendarUserCommandRepository                 calendarUserCommandRepository,
    IStringLocalizer<CreateReminderCommandHandler> localizer,
    ILogger<CreateReminderCommandHandler>          logger
)
    : IRequestHandler<CreateReminderCommand>
{
    public async Task Handle(CreateReminderCommand request, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();


        logger.LogInformation(
            "User {UserId} is initiating a reminder creation at {Timestamp}.",
            userId,
            DateTime.UtcNow);


        try
        {
            var targetUsers = request.TargetUsers?.Select(u => u.Id).ToList() ?? [];


            if (!targetUsers.Contains(userId)) targetUsers.Add(userId);


            var reminderDailyCalendar = request.Adapt<DailyCalendar>();
            reminderDailyCalendar.IsReminder = true;
            await dailyCalendarCommandRepository.AddAsync(reminderDailyCalendar, cancellationToken);


            var reminderUsers = targetUsers.Select(a => new CalendarUser
                {
                    CalendarId = reminderDailyCalendar.Id,
                    UserId     = a
                })
                .ToList();

            await calendarUserCommandRepository.AddRangeAsync(reminderUsers, cancellationToken);

            logger.LogInformation(
                "Reminder created successfully with CalendarId: {CalendarId} .",
                reminderDailyCalendar.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error creating reminder  for UserId: {UserId}. Error details: {ErrorMessage}",
                userId,
                ex.Message);

            throw SmartAttendanceException.InternalServerError(localizer["Reminder creation failed."].Value);
        }
    }
}