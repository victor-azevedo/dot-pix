using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Services;
using Microsoft.Extensions.Options;

namespace DotPixConciliationWorker;

public class ConciliationWorker(
    IQueueConsumerService<ConciliationMessageProcessor> conciliationConsumerService,
    ILogger<ConciliationWorker> logger,
    IOptions<AppParameters> options)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation($"Worker: {options.Value.WorkerName}");

            await conciliationConsumerService.StartConsuming("conciliation", stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}