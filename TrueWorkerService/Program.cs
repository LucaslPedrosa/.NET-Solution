using Application.Interfaces;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

        services.AddHttpClient();

        services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

        // Hangfire
        services.AddHangfire(config =>
            config.UseSqlServerStorage(connectionString));
        services.AddHangfireServer();


        // EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositórios e serviços
        services.AddScoped<IJokeRepository, JokeRepository>();
        services.AddScoped<JokeService>();

        // Worker
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
