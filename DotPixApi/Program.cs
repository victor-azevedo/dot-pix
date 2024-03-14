using System.Text.Json.Serialization;
using DotPixApi.Data;
using DotPixApi.Middlewares;
using DotPixApi.Repositories;
using DotPixApi.Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    string host = builder.Configuration["Database:Host"] ?? string.Empty;
    string port = builder.Configuration["Database:Port"] ?? string.Empty;
    string username = builder.Configuration["Database:Username"] ?? string.Empty;
    string database = builder.Configuration["Database:Name"] ?? string.Empty;
    string password = builder.Configuration["Database:Password"] ?? string.Empty;

    string connectionString = $"Host={host};Port={port};Username={username};Password={password};Database={database}";

    opts.UseNpgsql(connectionString, options => { options.MaxBatchSize(5_000); });
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

app.UseMiddleware<ExceptionHandleMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();

app.MapMetrics();

if (!app.Environment.IsProduction())
    app.Seed();

app.Run();