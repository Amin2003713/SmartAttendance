namespace SmartAttendance.Domain.Time.ir;

public class DayDetail
{
    [JsonProperty("index_in_base1")] public int IndexInBase1 { get; set; }

    [JsonProperty("index_in_base2")] public int IndexInBase2 { get; set; }

    [JsonProperty("index_in_base3")] public int IndexInBase3 { get; set; }

    [JsonProperty("enabled")] public bool Enabled { get; set; }

    [JsonProperty("is_holiday")] public bool IsHoliday { get; set; }

    [JsonProperty("is_weekend")] public bool IsWeekend { get; set; }

    [JsonProperty("is_today")] public bool IsToday { get; set; }

    [JsonProperty("row_index")] public int RowIndex { get; set; }

    [JsonProperty("column_index")] public int ColumnIndex { get; set; }

    [JsonProperty("object_name")] public string ObjectName { get; set; }
}