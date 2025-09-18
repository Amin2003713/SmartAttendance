using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Persistence.Services.Seeder;
using BackgroundJob = Hangfire.BackgroundJob;
using RecurringJob = Hangfire.RecurringJob;


namespace SmartAttendance.Persistence.Services.Taskes;

public class ScheduledTaskService(
    SeedCalendarService            calenderService,
    SmartAttendanceTenantDbContext context
) : IScopedDependency
{
    public void ScheduleTasks()
    {
        try
        {
            // Schedule SendDailySmsJobs to run every day at 9 AM if it doesn't exist
            // RecurringJob.AddOrUpdate("SendDailySmsJob", () => sendDailySmsJobs.Execute(), "0 9 * * *",
            //     new RecurringJobOptions()
            //     {
            //         TimeZone = TimeZoneInfo.Local, MisfireHandling = MisfireHandlingMode.Relaxed
            //     });

            // Schedule SeedCalenderService to run quarterly at 2 AM on the 1st of Jan, Apr, Jul, Oct
            RecurringJob.AddOrUpdate("SeedCalenderJob", () => calenderService.SeedCalender(), "0 2 1 1,4,7,10 *");


            BackgroundJob.Schedule(() => ActivateCalenderJob(),
                TimeSpan.FromSeconds(150
                ));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task ActivateCalenderJob()
    {
        if (!await context.TenantCalendars.AnyAsync())
            await calenderService.SeedCalender();
    }
}