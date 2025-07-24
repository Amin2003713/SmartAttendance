namespace Shifty.Domain.Time.ir;

public class EventDetail
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("base")] public int Base { get; set; }

    [JsonProperty("gregorian_year")] public int GregorianYear { get; set; }

    [JsonProperty("gregorian_month")] public int GregorianMonth { get; set; }

    [JsonProperty("gregorian_day")] public int GregorianDay { get; set; }

    [JsonProperty("gregorian_day_title")] public string GregorianDayTitle { get; set; }

    [JsonProperty("jalali_year")] public int JalaliYear { get; set; }

    [JsonProperty("jalali_month")] public int JalaliMonth { get; set; }

    [JsonProperty("jalali_day")] public int JalaliDay { get; set; }

    [JsonProperty("jalali_day_title")] public string JalaliDayTitle { get; set; }

    [JsonProperty("hijri_year")] public int HijriYear { get; set; }

    [JsonProperty("hijri_month")] public int HijriMonth { get; set; }

    [JsonProperty("hijri_day")] public int HijriDay { get; set; }

    [JsonProperty("hijri_day_title")] public string HijriDayTitle { get; set; }

    [JsonProperty("is_holiday")] public bool IsHoliday { get; set; }

    [JsonProperty("date_string")] public string DateString { get; set; }

    [JsonProperty("object_name")] public string ObjectName { get; set; }


    public DateTime BuildDate()
    {
        return new DateTime(GregorianYear, GregorianMonth, GregorianDay);
    }
}