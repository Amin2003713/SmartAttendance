namespace SmartAttendance.Domain.Time.ir;

public class MonthDetail
{
    [JsonProperty("month_index")] public int MonthIndex { get; set; }

    [JsonProperty("month_title")] public string MonthTitle { get; set; }

    [JsonProperty("year")] public int Year { get; set; }

    [JsonProperty("object_name")] public string ObjectName { get; set; }
}