using DotPixWorker.Interfaces;
using Microsoft.Extensions.Options;

namespace DotPixWorker;

public class Worker(
    IPaymentQueueConsumer paymentQueueConsumer,
    ILogger<Worker> logger,
    IOptions<AppParameters> options)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation($"Worker: {options.Value.WorkerName}");
            await paymentQueueConsumer.Watch(stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}