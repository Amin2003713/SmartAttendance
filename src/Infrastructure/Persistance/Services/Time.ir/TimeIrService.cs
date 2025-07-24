using System.Net.Http;
using Newtonsoft.Json;
using Shifty.Domain.Tenants;
using Shifty.Domain.Time.ir;

namespace Shifty.Persistence.Services.Time.ir;

public abstract class TimeIrService(
    HttpClient httpClient
) : IScopedDependency
{
    private const string ApiUrlTemplate =
        "https://api.time.ir/v1/event/fa/events/calendar?year={0}&month={1}&day=0&base1=0&base2=1&base3=2";

    private const string ApiKey = "ZAVdqwuySASubByCed5KYuYMzb9uB2f7";

    public async Task<List<TenantCalendar>> GetEventsForMonth(int year, int month)
    {
        var apiUrl = string.Format(ApiUrlTemplate, year, month);
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);

        var response = await httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var apiResponse  = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

        if (apiResponse?.Data is null)
            throw new NullReferenceException();


        var result = new List<TenantCalendar>();


        foreach (var day in apiResponse!.Data.DayList.OrderByDescending(a => a.IndexInBase1)
                     .DistinctBy(a => a.IndexInBase1)
                     .ToList())
        {
            var Calendar = new TenantCalendar
            {
                Date = apiResponse.Data.GetTodayDateTime(day.IndexInBase1)
            };

            if (day.IsHoliday)
                Calendar.IsHoliday = true;

            if (day.IsWeekend)
                Calendar.IsWeekend = true;

            var events = apiResponse.Data.GetEvent(Calendar.Date);

            if (events.Count != 0)
                foreach (var @event in events)
                {
                    Calendar.Details += @event + "\n";
                }

            result.Add(Calendar);
        }

        return result.OrderByDescending(a => a.Date).DistinctBy(a => a.Date).ToList();
    }
}