using DotPixWorker;
using DotPixWorker.Data;
using DotPixWorker.Interfaces;
using DotPixWorker.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContext<WorkerDbContext>(opts =>
{
    var connectionString = builder.Configuration["Database:ConnectionString"];
    opts.UseNpgsql(connectionString, options => { options.MaxBatchSize(5_000); });
});

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Services
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IConsumerPaymentQueue, ConsumerPaymentQueue>();
builder.Services.AddSingleton<IPspApiService, PspApiService>();

var host = builder.Build();
host.Run();