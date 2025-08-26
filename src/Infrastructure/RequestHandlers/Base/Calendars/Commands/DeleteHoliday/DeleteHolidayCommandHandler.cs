using Shifty.Application.Base.Calendars.Commands.DeleteHoliday;
using Shifty.Application.Interfaces.Calendars.CalendarProjects;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Base.Calendars.Commands.DeleteHoliday;

public class DeleteHolidayCommandHandler(
    IdentityService service,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    ICalendarProjectQueryRepository calendarProjectQueryRepository,
    IMediator mediator,
    ICalendarProjectCommandRepository calendarProjectCommandRepository,
    IDailyCalendarCommandRepository dailyCalendarCommandRepository,
    IStringLocalizer<DeleteHolidayCommandHandler> localizer,
    ILogger<DeleteHolidayCommandHandler> logger
)
    : IRequestHandler<DeleteHolidayCommand>
{
    public async Task Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();

        logger.LogInformation("User {UserId} requested deletion of holiday with Id {HolidayId} at {Timestamp}.",
            userId,
            request.HolidayId,
            DateTime.UtcNow);

        try
        {
            var holiday = await dailyCalendarQueryRepository.GetByIdAsync(cancellationToken, request.HolidayId);

            if (holiday == null)
            {
                logger.LogWarning("Holiday with Id {HolidayId} not found.", request.HolidayId);
                throw IpaException.NotFound(localizer["Not Found."].Value);
            }

            // Verify that the user is the creator of the holiday.
            if (holiday.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} attempted unauthorized deletion of holiday {HolidayId}.",
                    userId,
                    request.HolidayId);

                throw IpaException.BadRequest(localizer["Access denied."].Value);
            }

            // Retrieve the associated calendar project.
            var projectCalendar =
                await calendarProjectQueryRepository.GetCalendarProjectByCalendarId(request.HolidayId,
                    cancellationToken);

            if (projectCalendar is null)
            {
                logger.LogWarning("No calendar project found for holiday Id {HolidayId}.", request.HolidayId);
                throw IpaException.NotFound(localizer["Not Found."].Value);
            }

            // // Retrieve user access details for the project.
            // var access = await mediator.Send(
            //     new GetProjectUserQuery(projectCalendar.ProjectId, userId, ApplicationConstant.ServiceName),
            //     cancellationToken);
            //
            // if (!access.AccessList.HasAccess((int)TenantAccess.HolidayAccess) && holiday.IsHoliday)
            // {
            //     logger.LogWarning(
            //         "User {UserId} lacks holiday access for project {ProjectId} when attempting to delete holiday {HolidayId}.",
            //         userId,
            //         projectCalendar.ProjectId,
            //         request.HolidayId);
            //
            //     throw IpaException.BadRequest(localizer["Access denied."].Value);
            // }

            var associatedProjectsToDelete =
                await calendarProjectQueryRepository.GetListCalendarProjectByCalendarId(request.HolidayId,
                    cancellationToken);

            await calendarProjectCommandRepository.DeleteRangeAsync(associatedProjectsToDelete, cancellationToken);

            // Delete the holiday record.
            await dailyCalendarCommandRepository.DeleteAsync(holiday, cancellationToken);

            logger.LogInformation("Successfully deleted holiday with Id {HolidayId} for user {UserId}.",
                request.HolidayId,
                userId);
        }
        catch (IpaException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while deleting holiday with Id {HolidayId} for user {UserId}. Error details: {ErrorMessage}",
                request.HolidayId,
                userId,
                ex.Message);

            throw IpaException.InternalServerError(localizer["Unable to delete holiday."].Value);
        }
    }
}