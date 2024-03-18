using DotPixWorker;
using DotPixWorker.Data;
using DotPixWorker.Interfaces;
using DotPixWorker.Repositories;
using DotPixWorker.Services;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContextFactory<DotPixDbContext>();

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Services
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IPaymentProviderDestinyService, PaymentProviderDestinyService>();
builder.Services.AddSingleton<IPaymentProviderOriginService, PaymentProviderOriginService>();
builder.Services.AddSingleton<IPaymentQueueConsumer, PaymentQueueConsumer>();
builder.Services.AddSingleton<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IPspApiService, PspApiService>();

var host = builder.Build();
host.Run();