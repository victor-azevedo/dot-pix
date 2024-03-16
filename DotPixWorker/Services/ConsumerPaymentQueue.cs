using DotPixWorker.Interfaces;
using System.Text;
using System.Text.Json;
using DotPixWorker.Models.Dtos;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace DotPixWorker.Services;

public class ConsumerPaymentQueue(
    IOptions<AppParameters> options,
    IPaymentProviderDestinyService paymentProviderDestiny,
    IPaymentProviderOriginService paymentProviderOrigin,
    ILogger<ConsumerPaymentQueue> logger)
    : IConsumerPaymentQueue
{
    public async Task Watch(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.Value.RabbitMq.HostName
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "payment",
            durable: false, // TODO: change to durable
            exclusive: false,
            autoDelete: false,
            arguments: null);

        logger.LogInformation(" [*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            logger.LogInformation($" [x] Received {message}");

            var payment = JsonSerializer.Deserialize<InPaymentQueueDto>(message);
            if (payment == null)
                throw new ApplicationException("Invalid message from Payment Queue");

            await paymentProviderDestiny.HandlePaymentToDestiny(payment);

            await paymentProviderOrigin.HandlePaymentToOrigin(payment.Origin);
        };

        channel.BasicConsume(queue: "payment",
            autoAck: true, // TODO: remove auto ack
            consumer: consumer);

        logger.LogInformation(" Press \"Ctrl+C\" to exit.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Int32.MaxValue, stoppingToken);
        }
    }
}