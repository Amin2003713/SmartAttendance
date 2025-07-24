namespace Shifty.Application.Calendars.Commands.DeleteHoliday;

public record DeleteHolidayCommand(
    Guid HolidayId
) : IRequest;