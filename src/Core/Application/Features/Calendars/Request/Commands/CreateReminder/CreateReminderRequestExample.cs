namespace Shifty.Application.Features.Calendars.Request.Commands.CreateReminder;

public class CreateReminderRequestExample : IExamplesProvider<CreateReminderRequest>
{
    public CreateReminderRequest GetExamples()
    {
        return new CreateReminderRequest
        {
            Details = "یادآوری ",
            Date = DateTime.Today,
            TargetUsers =
                new List<UserTargetRequest>
                {
                    new()
                    {
                        Id = Guid.Empty,
                        Name = "نام کاربر "
                    }
                }
        };
    }
}