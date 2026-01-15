using BuildingBlocks.Jobs;
using BuildingBlocks.Jobs.Common;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.Pro.Redis;
using Hangfire.SqlServer;

namespace OrderService;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfireInternal(configuration);

        return services;
    }
    
    private static IServiceCollection AddHangfireInternal(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        if (string.IsNullOrEmpty(connectionString)) 
            throw new InvalidOperationException("Hangfire connection string is not configured");
        
        var storageOptions = new SqlServerStorageOptions { SchemaName = "hangfire" };

        // var redisConnectionString = configuration.GetConnectionString("Redis");
        // if (string.IsNullOrEmpty(redisConnectionString)) 
        //     throw new InvalidOperationException("Redis connection string is not configured");
        
        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            // .UseRecommendedSerializerSettings()
            .UseDynamicJobs()
            .UseSqlServerStorage(connectionString, storageOptions));
            // .UseRedisStorage(redisConnectionString, new RedisStorageOptions
            // {
            //     Prefix = "{test}:"
            // }));
            
        services.AddHangfireServer((sp, options) =>
        {
            options.Activator = new AspNetCoreJobActivator(sp.GetRequiredService<IServiceScopeFactory>());
            options.WorkerCount = Environment.ProcessorCount * 10;
            options.HeartbeatInterval = TimeSpan.FromSeconds(10);
            options.SchedulePollingInterval = TimeSpan.FromSeconds(1);
            options.Queues = ["critical", "default"];
            options.ServerName = Environment.MachineName;
        });

        services.AddSingleton<IJobRunner, JobRunner>();
        
        return services;
    }
}