using DotPixConciliationWorker;
using DotPixConciliationWorker.Data;
using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Options;
using DotPixConciliationWorker.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContextFactory<DotPixDbContext>();

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Message Broker
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("AppParameters:RabbitMq"));
builder.Services.AddSingleton<IConnectionFactory>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    return new ConnectionFactory() { HostName = options.HostName };
});

// Services
builder.Services.AddHostedService<ConciliationWorker>();

builder.Services.AddSingleton<IMessageProcessor<ConciliationMessageProcessor>, ConciliationMessageProcessor>();
builder.Services
    .AddSingleton<IQueueConsumerService<ConciliationMessageProcessor>,
        RabbitMqConsumerService<ConciliationMessageProcessor>>();

var host = builder.Build();
host.Run();