using System.Text.Json.Serialization;
using Shifty.Application.Base.Calendars.Request.Commands.CreateReminder;

namespace Shifty.Application.Base.Calendars.Request.Queries.GetReminder;

public class GetReminderResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Message { get; set; }

    public object Inserter { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserTargetRequest> TargetUsers { get; set; } = new();
}