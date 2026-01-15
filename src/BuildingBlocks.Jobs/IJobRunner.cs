namespace BuildingBlocks.Jobs;

public interface IJobRunner
{
    void Queue<TJob, TJobOptions>(TJobOptions configureJob)
        where TJobOptions : IJobOptions
        where TJob : IJob<TJobOptions>;

    void Schedule<TJob, TJobOptions>(TJobOptions configureJob,
        DateTimeOffset enqueueAt)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions;

    void Schedule<TJob, TJobOptions>(TJobOptions configureJob,
        DateTime enqueueAt)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions;

    void Schedule<TJob, TJobOptions>(TJobOptions configureJob,
        TimeSpan enqueueAt)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions;

    void Recurring<TJob, TJobOptions>(TJobOptions jobOption, int jobId, string cronExpression)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions;

    void Recurring<TJob, TJobOptions>(TJobOptions jobOption, string jobName, string cronExpression)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions;

    void Recurring<TJob, TJobOptions>(TJobOptions jobOption, string jobName, string cronExpression, string queue)
        where TJob : IJob<TJobOptions>
        where TJobOptions : IJobOptions;

    void Delete(string jobName);
}