using System.Text.Json.Serialization;
using DotPixApi.Data;
using DotPixApi.Interfaces;
using DotPixApi.Middlewares;
using DotPixApi.Options;
using DotPixApi.Repositories;
using DotPixApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prometheus;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    var connectionString = builder.Configuration["AppParameters:Database:ConnectionString"] ?? string.Empty;
    opts.UseNpgsql(connectionString, options => { options.MaxBatchSize(5_000); });
});

// Message Broker
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("AppParameters:RabbitMq"));
builder.Services.AddSingleton<IConnectionFactory>(serviceProvider =>
{
    var options = serviceProvider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    return new ConnectionFactory() { HostName = options.HostName };
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextService>();

builder.Services.AddScoped<HealthService>();

builder.Services.AddScoped<PaymentProviderTokenRepository>();

builder.Services.AddScoped<PixKeyService>();
builder.Services.AddScoped<PixKeyRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<PaymentProviderAccountService>();
builder.Services.AddScoped<PaymentProviderAccountRepository>();

builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<PaymentRepository>();

builder.Services.AddScoped<ConciliationService>();

builder.Services.AddScoped<IQueuePublisherService, RabbitMqPublisherService>();
builder.Services.AddScoped<PaymentQueuePublisher>();

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

// Avoid Circular Reference in JSON
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Metrics
app.UseMetricServer();
app.UseHttpMetrics(options => { options.AddCustomLabel("host", context => context.Request.Host.Host); });

app.UseHttpsRedirection();

app.MapControllers();

app.MapMetrics();

if (app.Environment.IsDevelopment())
    app.Seed();

app.UseMiddleware<ExceptionHandleMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.Run();