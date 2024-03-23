using DotPixConciliationWorker;
using DotPixConciliationWorker.Data;
using DotPixConciliationWorker.Exceptions;
using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Options;
using DotPixConciliationWorker.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Database
builder.Services.AddDbContextFactory<DotPixDbContext>();

// Message Broker
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("AppParameters:RabbitMq"));
builder.Services.AddSingleton<IModel>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    var connectionFactory = new ConnectionFactory { HostName = options.HostName };
    var connection = connectionFactory.CreateConnection();
    return connection.CreateModel();
});

// RabbitMq Consumer
builder.Services.AddSingleton<IQueueConsumerService<ConciliationMessageProcessor>,
    RabbitMqConsumerService<ConciliationMessageProcessor>>();
builder.Services.AddSingleton<IMessageProcessor<ConciliationMessageProcessor>,
    ConciliationMessageProcessor>();

builder.Services.AddSingleton<IQueueConsumerService<ConciliationQueueExceptions>,
    RabbitMqConsumerService<ConciliationQueueExceptions>>();
builder.Services.AddSingleton<IQueueExceptions<ConciliationQueueExceptions>,
    ConciliationQueueExceptions>();

// PSP Client
builder.Services.AddHttpClient<PspClient>();
builder.Services.AddSingleton<IPspClient, PspClient>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<PspClient>>();
    return new PspClient(logger);
});

// File Services 
builder.Services.AddSingleton<IConciliationFileProcessor, ConciliationFileProcessor>();
builder.Services.AddSingleton<IFileReader, NdjsonReader>();

// Worker
builder.Services.AddHostedService<ConciliationWorker>();

var host = builder.Build();
host.Run();
