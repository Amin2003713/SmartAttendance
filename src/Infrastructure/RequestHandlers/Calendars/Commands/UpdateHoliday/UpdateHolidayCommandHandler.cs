using Shifty.Application.Calendars.Commands.UpdateHoliday;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Calendars.Commands.UpdateHoliday;

public class UpdateHolidayCommandHandler(
    IdentityService service,
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    IDailyCalendarCommandRepository dailyCalendarCommandRepository,
    IStringLocalizer<UpdateHolidayCommandHandler> localizer,
    ILogger<UpdateHolidayCommandHandler> logger
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

                throw IpaException.NotFound(localizer["Holiday not found."].Value);
            }

            if (holiday.CreatedBy != userId)
            {
                logger.LogWarning("User {UserId} attempted to update a holiday {HolidayId} they did not create.",
                    userId,
                    request.HolidayId);

                throw IpaException.BadRequest(localizer["Access denied."].Value);
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
                        "Holiday move blocked: Target date {TargetDate} is already a holiday for ProjectId {ProjectId}",
                        request.Date,
                        request.ProjectId);

                    throw IpaException.BadRequest(localizer["The selected date is already marked as a holiday."].Value);
                }
            }

            holiday.Date = request.Date;
            holiday.Details = request.Details;
            holiday.IsHoliday = true;

            await dailyCalendarCommandRepository.UpdateAsync(holiday, cancellationToken);

            logger.LogInformation("Holiday {HolidayId} successfully updated by User {UserId}",
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
                "Unexpected error occurred while updating holiday {HolidayId} by User {UserId}",
                request.HolidayId,
                userId);

            throw IpaException.InternalServerError(localizer["Unable to update holiday."].Value);
        }
    }
}