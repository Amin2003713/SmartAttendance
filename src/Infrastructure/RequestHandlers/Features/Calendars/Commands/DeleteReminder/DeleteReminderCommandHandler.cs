using SmartAttendance.Application.Features.Calendars.Commands.DeleteReminder;
using SmartAttendance.Application.Interfaces.Calendars.CalendarUsers;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IdentityService service,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    IMediator mediator,
    ICalendarUserQueryRepository calendarUserQueryRepository,
    ICalendarUserCommandRepository calendarUserCommandRepository,
    IDailyCalendarCommandRepository dailyCalendarCommandRepository,
    IStringLocalizer<DeleteReminderCommandHandler> localizer,
    ILogger<DeleteReminderCommandHandler> logger
)
    : IRequestHandler<DeleteReminderCommand>
{
    public async Task Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();

        logger.LogInformation("User {UserId} initiated deletion for reminder {ReminderId}.",
            userId,
            request.ReminderId);

        try
        {
            var reminder = await dailyCalendarQueryRepository.GetByIdAsync(cancellationToken, request.ReminderId);

            if (reminder == null)
            {
                logger.LogWarning("Reminder {ReminderId} not found.", request.ReminderId);
                throw SmartAttendanceException.NotFound(localizer["Reminder not found."].Value);
            }

            if (reminder.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} attempted to delete reminder {ReminderId} not created by them.",
                    userId,
                    request.ReminderId);

                throw SmartAttendanceException.BadRequest(localizer["Access denied."].Value);
            }

            var associatedUsers =
                await calendarUserQueryRepository.GetCalendarUserByCalendarId(request.ReminderId, cancellationToken);

            await calendarUserCommandRepository.DeleteRangeAsync(associatedUsers, cancellationToken);

            await dailyCalendarCommandRepository.DeleteAsync(reminder, cancellationToken);

            logger.LogInformation("Successfully deleted reminder {ReminderId} by user {UserId}.",
                request.ReminderId,
                userId);
        }
        catch (SmartAttendanceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while deleting reminder {ReminderId} for user {UserId}.",
                request.ReminderId,
                userId);

            throw SmartAttendanceException.InternalServerError(localizer["Unable to delete reminder."].Value);
        }
    }
}