using Hangfire;
using SmartAttendance.Application.Interfaces.HangFire;

namespace SmartAttendance.Persistence.Repositories.HangFire;

public class HangFireJobRepository(
    ILogger<HangFireJobRepository>          logger,
    IStringLocalizer<HangFireJobRepository> localizer
) : IHangFireJobRepository
{
    public string AddFireAndForgetJob(Expression<Action> methodCall)
    {
        logger.LogInformation("Adding Fire-and-Forget job.");

        try
        {
            var jobId = BackgroundJob.Enqueue(methodCall);
            logger.LogInformation("Fire-and-Forget job added with ID: {JobId}", jobId);
            return jobId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to enqueue Fire-and-Forget job.");
            throw new InvalidOperationException(localizer["Failed to enqueue job."].Value);
        }
    }

    public string AddDelayedJob(Expression<Action> methodCall, TimeSpan delay)
    {
        logger.LogInformation("Scheduling delayed job with delay: {Delay}", delay);

        try
        {
            var jobId = BackgroundJob.Schedule(methodCall, delay);
            logger.LogInformation("Delayed job scheduled with ID: {JobId}", jobId);
            return jobId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to schedule delayed job.");
            throw new InvalidOperationException(localizer["Failed to schedule delayed job."].Value);
        }
    }


    public void AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cronExpression)
    {
        logger.LogInformation("Adding or updating recurring job: {JobId} with Cron: {Cron}",
            recurringJobId,
            cronExpression);

        try
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
            logger.LogInformation("Recurring job added or updated: {JobId}", recurringJobId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add or update recurring job: {JobId}", recurringJobId);
            throw new InvalidOperationException(localizer["Failed to add or update recurring job."].Value);
        }
    }

    public void RemoveRecurringJob(string recurringJobId)
    {
        logger.LogInformation("Removing recurring job: {JobId}", recurringJobId);

        try
        {
            RecurringJob.RemoveIfExists(recurringJobId);
            logger.LogInformation("Recurring job removed: {JobId}", recurringJobId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to remove recurring job: {JobId}", recurringJobId);
            throw new InvalidOperationException(localizer["Failed to remove recurring job."].Value);
        }
    }

    public void RequeueJob(string jobId)
    {
        logger.LogInformation("Requeuing job: {JobId}", jobId);

        try
        {
            BackgroundJob.Requeue(jobId);
            logger.LogInformation("Job requeued: {JobId}", jobId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to requeue job: {JobId}", jobId);
            throw new InvalidOperationException(localizer["Failed to requeue job."].Value);
        }
    }

    public void DeleteJob(string jobId)
    {
        logger.LogInformation("Deleting job: {JobId}", jobId);

        try
        {
            BackgroundJob.Delete(jobId);
            logger.LogInformation("Job deleted: {JobId}", jobId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete job: {JobId}", jobId);
            throw new InvalidOperationException(localizer["Failed to delete job."].Value);
        }
    }
}