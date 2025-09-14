namespace SmartAttendance.Application.Features.Calendars.Request.Commands.UpdateHoliday;

public class UpdateHolidayRequestExample : IExamplesProvider<UpdateHolidayRequest>
{
    public UpdateHolidayRequest GetExamples()
    {
        return new UpdateHolidayRequest
        {
            HolidayId = Guid.Empty,
            Details   = " ویرایش تعطیل",
            Date      = DateTime.Today
        };
    }
}