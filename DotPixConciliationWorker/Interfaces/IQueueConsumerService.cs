namespace DotPixConciliationWorker.Interfaces;

public interface IQueueConsumerService<TMessageProcessor>
{
    Task ConsumeMessage(string queueName, CancellationToken stoppingToken);
}