using Shifty.Application.Calendars.Request.Commands.CreateReminder;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Calendars.Request.Commands.UpdateReminder;

public class UpdateReminderRequestExample : IExamplesProvider<UpdateReminderRequest>
{
    public UpdateReminderRequest GetExamples()
    {
        return new UpdateReminderRequest
        {
            ReminderId = Guid.Empty,
            ProjectId = Guid.Empty,
            Details = "ویرایش یادآور",
            Date = DateTime.Today,
            TargetUsers = new List<UserTargetRequest>
            {
                new()
                {
                    Id = Guid.Empty, Name = "نام کاربر"
                }
            }
        };
    }
}