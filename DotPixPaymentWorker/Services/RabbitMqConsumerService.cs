using DotPixPaymentWorker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixPaymentWorker.Services;

public class RabbitMqConsumerService(
    IModel channel,
    IMessageProcessor messageProcessor,
    ILogger<RabbitMqConsumerService> logger)
    : IQueueConsumerService
{
    public async Task ConsumeMessage(string queueName, CancellationToken stoppingToken)
    {
        using (channel)
        {
            DeclareQueue(queueName);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) => await messageProcessor.ProcessMessageAsync(model, ea, channel);

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

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