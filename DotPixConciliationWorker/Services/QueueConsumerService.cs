using DotPixConciliationWorker.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Services;

public class QueueConsumerService<TMessageProcessor>(
    IMessageProcessor<ConciliationMessageProcessor> messageProcessor,
    IOptions<AppParameters> options,
    ILogger<QueueConsumerService<ConciliationMessageProcessor>> logger)
    : IQueueConsumerService<TMessageProcessor>
{
    public async Task StartConsuming(string queueName, CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = options.Value.RabbitMq.HostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        DeclareQueue(channel, queueName);

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) => await messageProcessor.ProcessMessageAsync(model, ea, channel);

        channel.BasicConsume(queue: "conciliation", autoAck: false, consumer: consumer);

        logger.LogInformation(" Press \"Ctrl+C\" to exit.");

        logger.LogInformation($"Started consuming messages from queue '{queueName}'");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private void DeclareQueue(IModel channel, string queueName)
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
}