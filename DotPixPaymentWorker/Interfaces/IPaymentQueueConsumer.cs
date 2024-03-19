namespace DotPixPaymentWorker.Interfaces;

public interface IPaymentQueueConsumer
{
    Task Watch(CancellationToken stoppingToken);
}