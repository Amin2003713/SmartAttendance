namespace Shifty.Application.Base.Calendars.Commands.DeleteHoliday;

public record DeleteHolidayCommand(
    Guid HolidayId
) : IRequest;