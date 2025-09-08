namespace SmartAttendance.Application.Features.Calendars.Commands.DeleteReminder;

public record DeleteReminderCommand(
    Guid ReminderId
) : IRequest;