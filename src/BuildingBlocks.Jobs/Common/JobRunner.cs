using Hangfire;

namespace BuildingBlocks.Jobs.Common;

public class JobRunner(IBackgroundJobClient backgroundJobClient) : IJobRunner
{
    private IBackgroundJobClient BackgroundJobClient { get; } = backgroundJobClient;

    public void Queue<TJob, TJobOptions>(TJobOptions configureJob)
        where TJob : IJob<TJobOptions> where TJobOptions : IJobOptions
    {
        BackgroundJobClient.Enqueue<TJob>(job => job.Perform(configureJob, null));
    }

    public void Schedule<TJob, TJobOptions>(TJobOptions configureJob, DateTimeOffset enqueueAt)
        where TJob : IJob<TJobOptions> where TJobOptions : IJobOptions
    {
        BackgroundJobClient.Schedule<TJob>(job => job.Perform(configureJob, null), enqueueAt);
    }

    public void Recurring<TJob, TJobOptions>(TJobOptions jobOptions, int jobId, string cronExpression)
        where TJob : IJob<TJobOptions> where TJobOptions : IJobOptions
    {
        RecurringJob.AddOrUpdate<TJob>(jobId.ToString(), job => job.Perform(jobOptions, null), cronExpression);
    }

    public void Recurring<TJob, TJobOptions>(TJobOptions jobOptions, string jobName, string cronExpression)
        where TJob : IJob<TJobOptions> where TJobOptions : IJobOptions
    {
        RecurringJob.AddOrUpdate<TJob>(jobName, job => job.Perform(jobOptions, null), cronExpression,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            });
    }

    public void Recurring<TJob, TJobOptions>(TJobOptions jobOptions, string jobName, string cronExpression,
        string queue)
        where TJob : IJob<TJobOptions> where TJobOptions : IJobOptions
    {
        RecurringJob.AddOrUpdate<TJob>(jobName, queue, job => job.Perform(jobOptions, null), cronExpression,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            });
    }

    public void Schedule<TJob, TJobOptions>(TJobOptions configureJob, DateTime enqueueAt)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions
    {
        BackgroundJobClient.Schedule<TJob>(job => job.Perform(configureJob, null), enqueueAt);
    }

    public void Schedule<TJob, TJobOptions>(TJobOptions configureJob, TimeSpan enqueueAt)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions
    {
        BackgroundJobClient.Schedule<TJob>(job => job.Perform(configureJob, null), enqueueAt);
    }

    public void Delete(string jobName)
    {
        try
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            IList<string> jobsScheduled = monitor.ScheduledJobs(0, int.MaxValue)
                .Where(x => x.Value.Job.Type.Name == jobName)
                .Select(x => x.Key)
                .ToList();
            foreach (var key in jobsScheduled)
            {
                BackgroundJob.Delete(key);
            }
        }
        catch
        {
            // ignored
        }
    }
}