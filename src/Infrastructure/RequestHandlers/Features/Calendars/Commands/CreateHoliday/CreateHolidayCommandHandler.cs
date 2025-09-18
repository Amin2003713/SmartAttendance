using Mapster;
using SmartAttendance.Application.Features.Calendars.Commands.CreateHoliday;
using SmartAttendance.Application.Interfaces.Calendars.DailyCalendars;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Calenders.DailyCalender;

namespace SmartAttendance.RequestHandlers.Features.Calendars.Commands.CreateHoliday;

public class CreateHolidayCommandHandler(
    IDailyCalendarQueryRepository                 dailyCalendarQueryRepository,
    IDailyCalendarCommandRepository               dailyCalendarCommandRepository,
    IStringLocalizer<CreateHolidayCommandHandler> localizer,
    ILogger<CreateHolidayCommandHandler>          logger
)
    : IRequestHandler<CreateHolidayCommand>
{
    public async Task Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting CreateHolidayCommand on Date: {Date}",
            request.Date);

        try
        {
            if (await dailyCalendarQueryRepository.IsAlreadyHoliday(request.Date, cancellationToken))
            {
                logger.LogWarning(
                    "Duplicate holiday creation attempt: A holiday already exists on Date: {Date}.",
                    request.Date);


                throw SmartAttendanceException.Conflict(localizer["Holiday already exists."].Value);
            }


            var holidayDailyCalendar = request.Adapt<DailyCalendar>();
            holidayDailyCalendar.IsHoliday = true;
            await dailyCalendarCommandRepository.AddAsync(holidayDailyCalendar, cancellationToken);


            logger.LogInformation(
                "Successfully created holiday for CalendarId: {CalendarId} Date: {Date}).",
                holidayDailyCalendar.Id,
                request.Date);
        }
        catch (Exception ex)
        {
            // Log detailed error with context, but throw a short, descriptive message.
            logger.LogError(ex,
                "Error creating holiday on Date: {Date}. Exception details: {ExceptionMessage}",
                request.Date,
                ex.Message);

            throw;
        }
    }
}