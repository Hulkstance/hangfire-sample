using BuildingBlocks.Jobs;
using OrderService;
using OrderService.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<RecurringJobsInitializer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.MapPost("/order", (IJobRunner jobRunner) =>
{
    jobRunner.Queue<CreateOrderJob, CreateOrderJobOptions>(
        new CreateOrderJobOptions(Guid.NewGuid()));
    
    return Results.Ok();
});

app.Run();
