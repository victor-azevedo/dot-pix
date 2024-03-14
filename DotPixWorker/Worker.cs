using DotPixWorker.Interfaces;

namespace DotPixWorker;

public class Worker(IConsumerPaymentQueue consumerPaymentQueue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumerPaymentQueue.WatchReceive(stoppingToken);
    }
}