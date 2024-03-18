namespace DotPixWorker.Interfaces;

public interface IPaymentQueueConsumer
{
    Task Watch(CancellationToken stoppingToken);
}