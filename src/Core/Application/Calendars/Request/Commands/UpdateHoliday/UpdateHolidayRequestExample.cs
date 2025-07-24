using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Calendars.Request.Commands.UpdateHoliday;

public class UpdateHolidayRequestExample : IExamplesProvider<UpdateHolidayRequest>
{
    public UpdateHolidayRequest GetExamples()
    {
        return new UpdateHolidayRequest
        {
            HolidayId = Guid.Empty, ProjectId = Guid.Empty, Details = " ویرایش تعطیل", Date = DateTime.Today
        };
    }
}