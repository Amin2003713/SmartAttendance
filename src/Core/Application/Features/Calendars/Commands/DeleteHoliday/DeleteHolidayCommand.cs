namespace Shifty.Application.Features.Calendars.Commands.DeleteHoliday;

public record DeleteHolidayCommand(
    Guid HolidayId
) : IRequest;