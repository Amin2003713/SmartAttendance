namespace Shifty.Application.Calendars.Commands.DeleteReminder;

public record DeleteReminderCommand(
    Guid ReminderId
) : IRequest;