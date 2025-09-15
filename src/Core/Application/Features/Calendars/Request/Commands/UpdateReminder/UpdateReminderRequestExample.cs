using SmartAttendance.Application.Features.Calendars.Request.Commands.CreateReminder;

namespace SmartAttendance.Application.Features.Calendars.Request.Commands.UpdateReminder;

public class UpdateReminderRequestExample : IExamplesProvider<UpdateReminderRequest>
{
    public UpdateReminderRequest GetExamples()
    {
        return new UpdateReminderRequest
        {
            ReminderId = Guid.Empty,
            Details    = "ویرایش یادآور",
            Date       = DateTime.Today,
            TargetUsers = new List<UserTargetRequest>
            {
                new UserTargetRequest
                {
                    Id   = Guid.Empty,
                    Name = "نام کاربر"
                }
            }
        };
    }
}