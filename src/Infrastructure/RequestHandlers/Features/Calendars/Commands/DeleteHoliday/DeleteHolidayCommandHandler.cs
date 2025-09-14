using SmartAttendance.Application.Features.Calendars.Commands.DeleteHoliday;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Commands.DeleteHoliday;

public class DeleteHolidayCommandHandler(
    IdentityService                               service,
    IDailyCalendarQueryRepository                 dailyCalendarQueryRepository,
    IMediator                                     mediator,
    IDailyCalendarCommandRepository               dailyCalendarCommandRepository,
    IStringLocalizer<DeleteHolidayCommandHandler> localizer,
    ILogger<DeleteHolidayCommandHandler>          logger
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
                throw SmartAttendanceException.NotFound(localizer["Not Found."].Value);
            }


            if (holiday.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} attempted unauthorized deletion of holiday {HolidayId}.",
                                  userId,
                                  request.HolidayId);

                throw SmartAttendanceException.BadRequest(localizer["Access denied."].Value);
            }

            await dailyCalendarCommandRepository.DeleteAsync(holiday, cancellationToken);

            logger.LogInformation("Successfully deleted holiday with Id {HolidayId} for user {UserId}.",
                                  request.HolidayId,
                                  userId);
        }
        catch (SmartAttendanceException)
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

            throw SmartAttendanceException.InternalServerError(localizer["Unable to delete holiday."].Value);
        }
    }
}