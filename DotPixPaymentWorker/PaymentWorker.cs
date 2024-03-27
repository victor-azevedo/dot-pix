using DotPixPaymentWorker.Interfaces;
using Microsoft.Extensions.Options;

namespace DotPixPaymentWorker;

public class PaymentWorker(
    IQueueConsumerService paymentQueueConsumer,
    ILogger<PaymentWorker> logger,
    IOptions<AppParameters> options)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation($"Worker: {options.Value.WorkerName}");
            await paymentQueueConsumer.ConsumeMessage("payment", stoppingToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}