using BuildingBlocks.Jobs;
using Hangfire;
using Hangfire.Server;

namespace OrderService.Jobs;

public sealed record CreateOrderJobOptions(Guid OrderId) : IJobOptions;

[AutomaticRetry(Attempts = 5)]
[Queue("critical")]
// [RetryInQueue("critical")]
public class CreateOrderJob(ILogger<CreateOrderJob> logger) : IJob<CreateOrderJobOptions>
{
    public Task Perform(CreateOrderJobOptions jobOptions, PerformContext? performContext)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Creating Order {OrderId}", jobOptions.OrderId);
        }

        return Task.CompletedTask;
    }
}