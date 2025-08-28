using Mapster;
using Shifty.Application.Base.Calendars.Commands.CreateHoliday;
using Shifty.Application.Interfaces.Calendars.CalendarProjects;
using Shifty.Application.Interfaces.Calendars.DailyCalendars;
using Shifty.Common.Exceptions;
using Shifty.Domain.Calenders.CalenderProjects;
using Shifty.Domain.Calenders.DailyCalender;

namespace Shifty.RequestHandlers.Base.Calendars.Commands.CreateHoliday;

public class CreateHolidayCommandHandler(
    IDailyCalendarQueryRepository dailyCalendarQueryRepository,
    IDailyCalendarCommandRepository dailyCalendarCommandRepository,
    ICalendarProjectCommandRepository calendarProjectCommandRepository,
    IStringLocalizer<CreateHolidayCommandHandler> localizer
)
    : IRequestHandler<CreateHolidayCommand>
{
    public async Task Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        // logger.LogInformation("Starting CreateHolidayCommand for ProjectId: {ProjectId} on Date: {Date}",
        //     request.ProjectId,
        //     request.Date);

        try
        {
            if (await dailyCalendarQueryRepository.IsAlreadyHoliday( request.Date, cancellationToken))
            {
                // Log detailed message for debugging.
                // logger.LogWarning(
                //     "Duplicate holiday creation attempt: A holiday already exists for ProjectId: {ProjectId} on Date: {Date}.",
                //     request.ProjectId,
                //     request.Date);

                // Throw a short message intended for the client.
                throw ShiftyException.Conflict(localizer["Holiday already exists."].Value);
            }

            // Map request data to DailyCalendar entity.
            var holidayDailyCalendar = request.Adapt<DailyCalendar>();
            holidayDailyCalendar.IsHoliday = true;
            await dailyCalendarCommandRepository.AddAsync(holidayDailyCalendar, cancellationToken);

            // Map request data to CalendarProject entity, linking it with the created DailyCalendar.
            var holidayProject = request.Adapt<CalendarProject>();
            holidayProject.CalendarId = holidayDailyCalendar.Id;
            await calendarProjectCommandRepository.AddAsync(holidayProject, cancellationToken);

            // logger.LogInformation(
            //     "Successfully created holiday for CalendarId: {CalendarId} (ProjectId: {ProjectId}, Date: {Date}).",
            //     holidayDailyCalendar.Id,
            //     request.ProjectId,
            //     request.Date);
        }
        catch (Exception ex)
        {
            // // Log detailed error with context, but throw a short, descriptive message.
            // logger.LogError(ex
            //     // "Error creating holiday for ProjectId: {ProjectId} on Date: {Date}. Exception details: {ExceptionMessage}",
            //     // request.ProjectId,
            //     // request.Date,
            //     // ex.Message);

            throw;
        }
    }
}