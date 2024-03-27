namespace DotPixPaymentWorker.Interfaces;

public interface IQueueConsumerService
{
    Task ConsumeMessage(string queueName, CancellationToken stoppingToken);
}