using System.Text.Json.Serialization;

namespace SmartAttendance.Application.Features.Calendars.Request.Queries.GetReminder;

public class GetReminderResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Message { get; set; }

    public object Inserter { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserTargetRequest> TargetUsers { get; set; } = [];
}