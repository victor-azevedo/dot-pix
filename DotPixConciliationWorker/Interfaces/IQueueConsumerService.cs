namespace DotPixConciliationWorker.Interfaces;

public interface IQueueConsumerService<TMessageProcessor>
{
    Task StartConsuming(string queueName, CancellationToken stoppingToken);
}