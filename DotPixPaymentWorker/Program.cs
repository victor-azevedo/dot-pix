using DotPixPaymentWorker;
using DotPixPaymentWorker.Data;
using DotPixPaymentWorker.Interfaces;
using DotPixPaymentWorker.Repositories;
using DotPixPaymentWorker.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Database
builder.Services.AddDbContextFactory<DotPixDbContext>();

// Message Broker
builder.Services.AddSingleton<IModel>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<AppParameters>>().Value;
    var connectionFactory = new ConnectionFactory { HostName = options.RabbitMq.HostName };
    var connection = connectionFactory.CreateConnection();
    return connection.CreateModel();
});

// RabbitMq Consumer
builder.Services.AddSingleton<IQueueConsumerService, RabbitMqConsumerService>();
builder.Services.AddSingleton<IMessageProcessor, PaymentMessageProcessor>();

// Services
builder.Services.AddSingleton<IPaymentProviderDestinyService, PaymentProviderDestinyService>();
builder.Services.AddSingleton<IPaymentProviderOriginService, PaymentProviderOriginService>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPspApiService, PspApiService>();

// Worker
builder.Services.AddHostedService<PaymentWorker>();

var host = builder.Build();
host.Run();