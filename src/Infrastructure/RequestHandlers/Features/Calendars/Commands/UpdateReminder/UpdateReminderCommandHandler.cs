using Shifty.Application.Features.Calendars.Commands.UpdateReminder;
using Shifty.Application.Interfaces.Calendars.CalendarUsers;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Common.General.Enums.Access;
using Shifty.Domain.Calenders.CalenderUsers;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Calendars.Commands.UpdateReminder;

public class UpdateReminderCommandHandler(
    IdentityService service,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    IMediator mediator,
    ICalendarUserCommandRepository calendarUserCommandRepository,
    IDailyCalendarCommandRepository dailyCalendarCommandRepository,
    IStringLocalizer<UpdateReminderCommandHandler> localizer,
    ICalendarUserQueryRepository calendarUserQueryRepository,
    ILogger<UpdateReminderCommandHandler> logger
)
    : IRequestHandler<UpdateReminderCommand>
{
    public async Task Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();
        logger.LogInformation("User {UserId} is updating reminder {ReminderId}", userId, request.ReminderId);

        try
        {
            var reminder = await dailyCalendarQueryRepository.GetByIdAsync(cancellationToken, request.ReminderId);

            if (reminder == null)
            {
                logger.LogWarning("Reminder {ReminderId} not found for user {UserId}", request.ReminderId, userId);
                throw ShiftyException.BadRequest(localizer["Reminder not found."].Value);
            }

            if (reminder.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} tried to edit reminder {ReminderId} not created by them.",
                    userId,
                    request.ReminderId);

                throw ShiftyException.BadRequest(localizer["Access denied."].Value);
            }


            var targetUsers = request.TargetUsers.Select(uid => uid.Id).ToList();

            if (!targetUsers.Contains(userId))
                targetUsers.Add(userId);

            reminder.Date = request.Date;
            reminder.Details = request.Details;
            reminder.IsReminder = true;

            await dailyCalendarCommandRepository.UpdateAsync(reminder, cancellationToken);

            var calendarUser =
                await calendarUserQueryRepository.GetCalendarUserByCalendarId(request.ReminderId, cancellationToken);

            await calendarUserCommandRepository.DeleteRangeAsync(calendarUser, cancellationToken);

            var reminderUsers = targetUsers
                .Select(guid => new CalendarUser
                {
                    UserId = guid,
                    CalendarId = request.ReminderId
                })
                .ToList();

            await calendarUserCommandRepository.AddRangeAsync(reminderUsers, cancellationToken);

            logger.LogInformation("Reminder {ReminderId} successfully updated by User {UserId}",
                request.ReminderId,
                userId);
        }
        catch (ShiftyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while updating reminder {ReminderId} by user {UserId}",
                request.ReminderId,
                userId);

            throw ShiftyException.InternalServerError(localizer["Unable to update reminder."].Value);
        }
    }
}