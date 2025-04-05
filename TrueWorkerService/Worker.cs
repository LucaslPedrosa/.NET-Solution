using Hangfire;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerService.Services;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        RecurringJob.AddOrUpdate<JokeService>(
            "fetch-joke",
            service => service.FetchAndStoreJokeAsync(),
            "*/0 0 1 * * *"); // Seconds | Minutes | Hours | Day of Month | Month | Day of Week

        return Task.CompletedTask;
    }
}