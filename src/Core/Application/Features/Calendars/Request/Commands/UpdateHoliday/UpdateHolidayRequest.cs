namespace SmartAttendance.Application.Features.Calendars.Request.Commands.UpdateHoliday;

public class UpdateHolidayRequest
{
    public Guid HolidayId { get; set; }
    public string Details { get; set; }
    public DateTime Date { get; set; }
}