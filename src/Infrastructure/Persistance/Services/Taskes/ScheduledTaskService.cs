using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.PlanEnrollments;
using SmartAttendance.Domain.Features.Plans;
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


public class AutoMarkAbsentJob(
    SmartAttendanceDbContext DbContext,
    ILogger<AutoMarkAbsentJob> logger
)  : IScopedDependency
{
    public DbSet<Plan> Plans => DbContext.Set<Plan>()  ;
    public DbSet<PlanEnrollment> PlanEnrollments => DbContext.Set<PlanEnrollment>()  ;

    public async Task RunForEndedPlans(Guid plan , CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var endedPlans = await Plans
            .Where(p => p.IsActive && p.Id == plan)
            .Select(p => p.Id)
            .ToListAsync(ct);

        if (!endedPlans.Any())
        {
            logger.LogInformation("No recently ended plans found.");
            return;
        }

        var totalMarked = 0;

        foreach (var planId in endedPlans)
        {
            var marked = await MarkAbsentForPlan(planId, ct);
            totalMarked += marked;
        }

        logger.LogInformation("AutoMarkAbsentJob completed. Marked {Count} students as absent.", totalMarked);
    }

    private async Task<int> MarkAbsentForPlan(Guid planId, CancellationToken ct)
    {
        var enrollmentsWithoutAttendance = await PlanEnrollments .Include(a=>a.Plan)
            .Where(e => e.PlanId == planId &&
                        e.IsActive &&
                        e.Attendance == null && e.Plan.EndTime > DateTime.Now) // No attendance record
            .ToListAsync(ct);

        if (!enrollmentsWithoutAttendance.Any())
            return 0;

        var attendanceRecords = enrollmentsWithoutAttendance.Select(e => new Attendance
            {
                Id = Guid.NewGuid(),
                EnrollmentId = e.Id,
                Status = 0, // 0 = Absent
                RecordedAt = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = null // System job, or use a system user GUID
                // You can add ModifiedBy/DeletedBy = null
            })
            .ToList();

        DbContext.AddRange(attendanceRecords);
        await DbContext.SaveChangesAsync(ct);

        return attendanceRecords.Count;
    }
}