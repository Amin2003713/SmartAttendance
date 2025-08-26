namespace Shifty.Application.Base.Calendars.Request.Commands.UpdateHoliday;

public class UpdateHolidayRequest
{
    public Guid HolidayId { get; set; }
    public string Details { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime Date { get; set; }
}