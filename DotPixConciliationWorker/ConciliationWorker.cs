namespace DotPixConciliationWorker;

public class ConciliationWorker : BackgroundService
{
    private readonly ILogger<ConciliationWorker> _logger;

    public ConciliationWorker(ILogger<ConciliationWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}