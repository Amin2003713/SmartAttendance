using SmartAttendance.Application.Features.Calendars.Commands.UpdateHoliday;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Commands.UpdateHoliday;

public class UpdateHolidayCommandHandler(
    IdentityService                               service,
    IDailyCalendarQueryRepository                 dailyCalendarQueryRepository,
    IDailyCalendarCommandRepository               dailyCalendarCommandRepository,
    IStringLocalizer<UpdateHolidayCommandHandler> localizer,
    ILogger<UpdateHolidayCommandHandler>          logger
)
    : IRequestHandler<UpdateHolidayCommand>
{
    public async Task Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();

        logger.LogInformation("User {UserId} initiated holiday update for HolidayId {HolidayId}",
            userId,
            request.HolidayId);

        try
        {
            var holiday = await dailyCalendarQueryRepository.GetByIdAsync(cancellationToken, request.HolidayId);

            if (holiday == null)
            {
                logger.LogWarning("Holiday {HolidayId} not found during update attempt by User {UserId}",
                    request.HolidayId,
                    userId);

                throw SmartAttendanceException.NotFound(localizer["Holiday not found."].Value);
            }

            if (holiday.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} attempted to update a holiday {HolidayId} they did not create.",
                    userId,
                    request.HolidayId);

                throw SmartAttendanceException.BadRequest(localizer["Access denied."].Value);
            }

            var isSameDate = holiday.Date == request.Date;

            if (!isSameDate)
            {
                var isAlreadyHoliday =
                    await dailyCalendarQueryRepository.IsAlreadyHoliday(
                        request.Date,
                        cancellationToken);

                if (isAlreadyHoliday)
                {
                    logger.LogWarning(
                        "Holiday move blocked: Target date {TargetDate} is already a holiday ",
                        request.Date);

                    throw SmartAttendanceException.BadRequest(localizer["The selected date is already marked as a holiday."].Value);
                }
            }

            holiday.Date      = request.Date;
            holiday.Details   = request.Details;
            holiday.IsHoliday = true;

            await dailyCalendarCommandRepository.UpdateAsync(holiday, cancellationToken);

            logger.LogInformation("Holiday {HolidayId} successfully updated by User {UserId}",
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
                "Unexpected error occurred while updating holiday {HolidayId} by User {UserId}",
                request.HolidayId,
                userId);

            throw SmartAttendanceException.InternalServerError(localizer["Unable to update holiday."].Value);
        }
    }
}