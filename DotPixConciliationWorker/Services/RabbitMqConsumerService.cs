using DotPixConciliationWorker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Services;

public class RabbitMqConsumerService<TMessageProcessor>(
    IModel channel,
    IMessageProcessor<ConciliationMessageProcessor> messageProcessor,
    ILogger<RabbitMqConsumerService<ConciliationMessageProcessor>> logger)
    : IQueueConsumerService<TMessageProcessor>
{
    public async Task ConsumeMessage(string queueName, CancellationToken stoppingToken)
    {
        using (channel)
        {
            DeclareQueue(queueName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) => await messageProcessor.ProcessMessageAsync(model, ea, channel);

            channel.BasicConsume(queue: "conciliation", autoAck: false, consumer: consumer);

            logger.LogInformation(" Press \"Ctrl+C\" to exit.");

            logger.LogInformation($"Started consuming messages from queue '{queueName}'");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }

    private void DeclareQueue(string queueName)
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
}
