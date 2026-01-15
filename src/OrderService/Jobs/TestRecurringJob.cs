using BuildingBlocks.Jobs;
using Hangfire.Server;

namespace OrderService.Jobs;

public sealed record TestRecurringJobOptions : IJobOptions;

public class TestRecurringJob(ILogger<TestRecurringJob> logger) : IJob<TestRecurringJobOptions>
{
    public Task Perform(TestRecurringJobOptions jobOptions, PerformContext? performContext)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Monitor ERP parked requests job started");
        }

        return Task.CompletedTask;
    }
}