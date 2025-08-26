namespace Shifty.Application.Base.Calendars.Request.Commands.CreateHoliday;

public class CreateHolidayRequest
{
    public string Details { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime Date { get; set; }
}