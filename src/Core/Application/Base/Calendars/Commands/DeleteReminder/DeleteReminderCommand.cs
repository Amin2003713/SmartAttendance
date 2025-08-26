namespace Shifty.Application.Base.Calendars.Commands.DeleteReminder;

public record DeleteReminderCommand(
    Guid ReminderId
) : IRequest;