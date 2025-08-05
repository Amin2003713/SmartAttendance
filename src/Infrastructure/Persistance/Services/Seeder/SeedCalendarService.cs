using System.Globalization;
using DNTPersianUtils.Core;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Persistence.Services.Time.ir;


// using PersianTools.Core;

namespace Shifty.Persistence.Services.Seeder;

public class SeedCalendarService(
    ShiftyTenantDbContext db,
    TimeIrService service
) : ITransientDependency
{
    public async Task SeedCalender()
    {
        var now                = DateTime.UtcNow;
        var persianCalendar    = new PersianCalendar();
        var currentPersianYear = persianCalendar.GetYear(now);

        var start = currentPersianYear - 21;
        var end   = currentPersianYear + 21;

        for (var year = start; year < end; year++)
            for (var i = 1; i <= 12; i++)
            {
                var persianDate = new PersianDateTime(year, i, 1);
                var data        = await service.GetEventsForMonth(persianDate.Year!.Value, persianDate.Month!.Value);

                if (data is null) return;

                foreach (var dailyCalender in data)
                {
                    var calender = await db.TenantCalendars.FirstOrDefaultAsync(a =>
                        a.Date == dailyCalender.Date &&
                        a.IsHoliday == dailyCalender.IsHoliday &&
                        a.IsWeekend == dailyCalender.IsWeekend);

                    if (calender is not null) continue;

                    var update = await db.TenantCalendars.FirstOrDefaultAsync(a =>
                        a.Date == dailyCalender.Date);

                    if (update is null)
                    {
                        db.TenantCalendars.Add(dailyCalender);
                        continue;
                    }

                    update.IsHoliday = dailyCalender.IsHoliday;
                    update.IsWeekend = dailyCalender.IsWeekend;
                    update.Details = dailyCalender.Details!.Trim();

                    db.Entry(update).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();
            }
    }
}