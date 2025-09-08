using System.Text.Json.Serialization;

namespace SmartAttendance.Application.Features.Calendars.Request.Queries.GetHoliday;

public class GetHolidayResponse
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object Inserter { get; set; }

    public bool? IsCustom { get; set; }
}