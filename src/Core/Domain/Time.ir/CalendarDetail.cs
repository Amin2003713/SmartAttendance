namespace Shifty.Domain.Time.ir;

public class CalendarDetail
{
    [JsonProperty("base")] public int Base { get; set; }

    [JsonProperty("month_list")] public required List<MonthDetail> MonthList { get; set; }

    [JsonProperty("year_list")] public required List<int> YearList { get; set; }

    [JsonProperty("object_name")] public required string ObjectName { get; set; }

    public MonthDetail GetPersianMonthYear()
    {
        return MonthList.FirstOrDefault()!;
    }
}