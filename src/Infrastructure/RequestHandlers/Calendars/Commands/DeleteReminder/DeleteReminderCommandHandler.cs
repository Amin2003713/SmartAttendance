using Shifty.Application.Calendars.Commands.DeleteReminder;
using Shifty.Application.Interfaces.Calendars.CalendarProjects;
using Shifty.Application.Interfaces.Calendars.CalendarUsers;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Common.General.Enums.Access;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Calendars.Commands.DeleteReminder;

public class DeleteReminderCommandHandler(
    IdentityService service,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    ICalendarProjectQueryRepository calendarProjectQueryRepository,
    IMediator mediator,
    ICalendarUserQueryRepository calendarUserQueryRepository,
    ICalendarUserCommandRepository calendarUserCommandRepository,
    ICalendarProjectCommandRepository calendarProjectCommandRepository,
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
                throw IpaException.NotFound(localizer["Reminder not found."].Value);
            }

            if (reminder.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} attempted to delete reminder {ReminderId} not created by them.",
                    userId,
                    request.ReminderId);

                throw IpaException.BadRequest(localizer["Access denied."].Value);
            }

            var projectCalendar =
                await calendarProjectQueryRepository.GetCalendarProjectByCalendarId(request.ReminderId,
                    cancellationToken);

            if (projectCalendar is null)
            {
                logger.LogWarning("Calendar project not found for reminder {ReminderId}.", request.ReminderId);
                throw IpaException.NotFound(localizer["Reminder not found."].Value);
            }

            var associatedUsers =
                await calendarUserQueryRepository.GetCalendarUserByCalendarId(request.ReminderId, cancellationToken);

            // var access = await mediator.Send(
            //     new GetProjectUserQuery(projectCalendar.ProjectId, userId, ApplicationConstant.ServiceName),
            //     cancellationToken);
            //
            //
            // // Validate permission when deleting reminder involving other users
            // if (!access.AccessList.HasAccess((int)TenantAccess.ReminderAccess) &&
            //     reminder.IsReminder &&
            //     associatedUsers.Any(u => u.UserId != userId))
            // {
            //     logger.LogWarning(
            //         "User {UserId} tried to delete a reminder shared with others without proper access. ReminderId: {ReminderId}",
            //         userId,
            //         request.ReminderId);
            //
            //     throw IpaException.BadRequest(localizer["Access denied."].Value);
            // }

            // Perform deletions
            await calendarUserCommandRepository.DeleteRangeAsync(associatedUsers, cancellationToken);

            var associatedProjects =
                await calendarProjectQueryRepository.GetListCalendarProjectByCalendarId(request.ReminderId,
                    cancellationToken);

            await calendarProjectCommandRepository.DeleteRangeAsync(associatedProjects, cancellationToken);

            await dailyCalendarCommandRepository.DeleteAsync(reminder, cancellationToken);

            logger.LogInformation("Successfully deleted reminder {ReminderId} by user {UserId}.",
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
                "Unexpected error while deleting reminder {ReminderId} for user {UserId}.",
                request.ReminderId,
                userId);

            throw IpaException.InternalServerError(localizer["Unable to delete reminder."].Value);
        }
    }
}