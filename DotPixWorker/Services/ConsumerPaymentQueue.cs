using DotPixWorker.Interfaces;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace DotPixWorker.Services;

public class ConsumerPaymentQueue(ILogger<ConsumerPaymentQueue> logger) : IConsumerPaymentQueue
{
    public void WatchReceive(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "payment",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        logger.LogInformation(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            logger.LogInformation($" [x] Received {message}");
        };

        channel.BasicConsume(queue: "payment",
            autoAck: true,
            consumer: consumer);

        logger.LogInformation(" Press \"Ctrl+C\" to exit.");
        while (!stoppingToken.IsCancellationRequested)
        {
        }
    }
}