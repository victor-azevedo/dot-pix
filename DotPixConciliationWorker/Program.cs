using DotPixConciliationWorker;
using DotPixConciliationWorker.Data;
using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Services;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContextFactory<DotPixDbContext>();

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Services
builder.Services.AddHostedService<ConciliationWorker>();

builder.Services.AddSingleton<IMessageProcessor<ConciliationMessageProcessor>, ConciliationMessageProcessor>();
builder.Services
    .AddSingleton<IQueueConsumerService<ConciliationMessageProcessor>,
        QueueConsumerService<ConciliationMessageProcessor>>();

var host = builder.Build();
host.Run();