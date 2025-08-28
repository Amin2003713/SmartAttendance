using Shifty.Application.Features.Calendars.Request.Commands.CreateReminder;

namespace Shifty.Application.Features.Calendars.Request.Commands.UpdateReminder;

public class UpdateReminderRequestExample : IExamplesProvider<UpdateReminderRequest>
{
    public UpdateReminderRequest GetExamples()
    {
        return new UpdateReminderRequest
        {
            ReminderId = Guid.Empty,
            Details = "ویرایش یادآور",
            Date = DateTime.Today,
            TargetUsers = new List<UserTargetRequest>
            {
                new()
                {
                    Id = Guid.Empty,
                    Name = "نام کاربر"
                }
            }
        };
    }
}