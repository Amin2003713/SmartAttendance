using Mapster;
using Shifty.Application.Calendars.Commands.UpdateReminder;
using Shifty.Application.Interfaces.Calendars.CalendarUsers;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Domain.Calenders.CalenderUsers;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Calendars.Commands.UpdateReminder;

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
                throw IpaException.BadRequest(localizer["Reminder not found."].Value);
            }

            if (reminder.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} tried to edit reminder {ReminderId} not created by them.",
                    userId,
                    request.ReminderId);

                throw IpaException.BadRequest(localizer["Access denied."].Value);
            }


            var targetUsers =
                // access.AccessList.HasAccess((int)TenantAccess.ReminderAccess)
                // ? request.TargetUsers.Select(uid => uid.Id).ToList()// todo 
                // :
                new List<Guid>
                {
                    userId
                };

            if (!targetUsers.Contains(userId))
                targetUsers.Add(userId);

            // Update reminder data
            reminder.Date = request.Date;
            reminder.Details = request.Details;
            reminder.IsReminder = true;

            await dailyCalendarCommandRepository.UpdateAsync(reminder, cancellationToken);

            var calendarUser =
                await calendarUserQueryRepository.GetCalendarUserByCalendarId(request.ReminderId, cancellationToken);

            await calendarUserCommandRepository.DeleteRangeAsync(calendarUser, cancellationToken);

            var reminderUsers = targetUsers.Adapt<List<CalendarUser>>()
                .Select(user =>
                {
                    user.CalendarId = request.ReminderId;
                    return user;
                })
                .ToList();

            await calendarUserCommandRepository.AddRangeAsync(reminderUsers, cancellationToken);

            logger.LogInformation("Reminder {ReminderId} successfully updated by User {UserId}",
                request.ReminderId,
                userId);
        }
        catch (IpaException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while updating reminder {ReminderId} by user {UserId}",
                request.ReminderId,
                userId);

            throw IpaException.InternalServerError(localizer["Unable to update reminder."].Value);
        }
    }
}