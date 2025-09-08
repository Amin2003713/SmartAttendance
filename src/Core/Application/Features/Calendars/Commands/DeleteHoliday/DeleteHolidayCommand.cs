namespace SmartAttendance.Application.Features.Calendars.Commands.DeleteHoliday;

public record DeleteHolidayCommand(
    Guid HolidayId
) : IRequest;