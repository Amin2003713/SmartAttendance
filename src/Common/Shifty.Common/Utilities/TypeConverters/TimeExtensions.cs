namespace Shifty.Common.Utilities.TypeComverters;

public static class TimeExtensions
{
    public static string CalculateWorkingHour(this DateTime enterTime, DateTime exitTime)
    {
        if (enterTime == default! || exitTime == default!)
            return "00:00";

        var timeSpan = exitTime - enterTime;
        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
    }
}