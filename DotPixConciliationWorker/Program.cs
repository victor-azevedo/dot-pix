using DotPixConciliationWorker;
using DotPixConciliationWorker.Data;

var builder = Host.CreateApplicationBuilder(args);

// Database
builder.Services.AddDbContextFactory<DotPixDbContext>();

// Environment variables config
IConfiguration config = builder.Configuration;
builder.Services.Configure<AppParameters>(config.GetSection("AppParameters"));

// Services
builder.Services.AddHostedService<ConciliationWorker>();

var host = builder.Build();
host.Run();