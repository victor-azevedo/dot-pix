using DotPixWorker.Interfaces;
using Microsoft.Extensions.Options;

namespace DotPixWorker;

public class Worker(
    IConsumerPaymentQueue consumerPaymentQueue,
    IPspApiService pspApiService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        pspApiService.GetHealth();
        consumerPaymentQueue.WatchReceive(stoppingToken);
    }
}