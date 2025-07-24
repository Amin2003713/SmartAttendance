using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Calendars.Request.Commands.CreateReminder;

public class CreateReminderRequestExample : IExamplesProvider<CreateReminderRequest>
{
    public CreateReminderRequest GetExamples()
    {
        return new CreateReminderRequest
        {
            Details = "یادآوری ",
            ProjectId = Guid.Empty,
            Date = DateTime.Today,
            TargetUsers =
                new List<UserTargetRequest>
                {
                    new()
                    {
                        Id = Guid.Empty, Name = "نام کاربر "
                    }
                }
        };
    }
}