using Hangfire;
using OrderService.Jobs;

namespace OrderService;

public class RecurringJobsInitializer(IRecurringJobManager recurringJobManager, IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        
        recurringJobManager.AddOrUpdateDynamic<TestRecurringJob>(
            "TestRecurring",
            "critical",
            job => job.Perform(new TestRecurringJobOptions(), null),
            "0 0 * * 6");
        
        return Task.CompletedTask;
    }
}
