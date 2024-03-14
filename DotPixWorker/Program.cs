using DotPixWorker;
using DotPixWorker.Data;
using DotPixWorker.Interfaces;
using DotPixWorker.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContext<WorkerDbContext>(opts =>
{
    var connectionString = builder.Configuration["ConnectionStrings:DotpixDatabase"];
    opts.UseNpgsql(connectionString, options => { options.MaxBatchSize(5_000); });
});

// Services
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IConsumerPaymentQueue, ConsumerPaymentQueue>();

var host = builder.Build();
host.Run();