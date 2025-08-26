namespace Shifty.Application.Base.Calendars.Request.Commands.CreateHoliday;

public class CreateHolidayRequestExample : IExamplesProvider<CreateHolidayRequest>
{
    public CreateHolidayRequest GetExamples()
    {
        return new CreateHolidayRequest
        {
            Date = DateTime.Today,
            Details = "تعطیل"
        };
    }
}