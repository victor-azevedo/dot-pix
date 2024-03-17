namespace DotPixWorker.Interfaces;

public interface IConsumerPaymentQueue
{
    Task Watch(CancellationToken stoppingToken);
}