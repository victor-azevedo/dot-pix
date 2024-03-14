namespace DotPixWorker.Interfaces;

public interface IConsumerPaymentQueue
{
    void WatchReceive(CancellationToken stoppingToken);
}