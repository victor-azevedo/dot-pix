using DotPixWorker.Interfaces;
using Microsoft.Extensions.Options;

namespace DotPixWorker;

public class Worker(
    IConsumerPaymentQueue consumerPaymentQueue,
    ILogger<Worker> logger,
    IOptions<AppParameters> options)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation($"Worker: {options.Value.WorkerName}");
            await consumerPaymentQueue.Watch(stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}