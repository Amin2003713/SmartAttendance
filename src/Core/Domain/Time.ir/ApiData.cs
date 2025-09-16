namespace SmartAttendance.Domain.Time.ir;

public class ApiData
{
    // Private backing field
    private List<DayDetail> _dayList;

    [JsonProperty("year")] public int Year { get; set; }

    [JsonProperty("month")] public int Month { get; set; }

    [JsonProperty("day")] public int Day { get; set; }

    [JsonProperty("base1")] public int Base1 { get; set; }

    [JsonProperty("base2")] public int Base2 { get; set; }

    [JsonProperty("base3")] public int Base3 { get; set; }

    [JsonProperty("created_date")] public DateTime CreatedDate { get; set; }

    [JsonProperty("calendar_detail_list")] public List<CalendarDetail> CalendarDetailList { get; set; }

    [JsonProperty("day_list")] public List<DayDetail> DayList
    {
        get { return _dayList?.Where(a => a.Enabled)!.ToList()!; }
        set => _dayList = value;
    }

    [JsonProperty("event_list")] public List<EventDetail> EventList { get; set; }

    [JsonProperty("object_name")] public string ObjectName { get; set; }


    public DateTime GetTodayDateTime(int day, int calendarDetailsBase = 0)
    {
        var yearAndMonth = CalendarDetailList.FirstOrDefault(a => a.Base == calendarDetailsBase)!.GetPersianMonthYear();

        var lastday = ConvertPersianToGregorian(yearAndMonth.Year, yearAndMonth.MonthIndex, 1).GetPersianMonthStartAndEndDates();

        var date = ConvertPersianToGregorian(yearAndMonth.Year,
                                             yearAndMonth.MonthIndex,
                                             Math.Min(day, lastday.LastDayNumber));

        return date;
    }

    protected static DateTime ConvertPersianToGregorian(int persianYear, int persianMonth, int persianDay)
    {
        var persianCalendar = new PersianCalendar();
        var gregorianDate   = persianCalendar.ToDateTime(persianYear, persianMonth, persianDay, 0, 0, 0, 0);
        return gregorianDate;
    }

    public List<string> GetEvent(DateTime date)
    {
        return EventList.Where(a => a.BuildDate() == date).Select(a => a.Title).ToList();
    }
}