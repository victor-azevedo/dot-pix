using System.Text;
using System.Text.Json;
using DotPixApi.Models;
using DotPixApi.Models.Dtos;
using RabbitMQ.Client;

namespace DotPixApi.Services;

public class PaymentQueuePublisher(IConfiguration config)
{
    public void Send(Payments payment, DateTime expireAt)
    {
        var factory = new ConnectionFactory { HostName = config["AppParameters:RabbitMq:HostName"] };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var basicProperties = channel.CreateBasicProperties();
        basicProperties.Persistent = true;

        channel.QueueDeclare(
            queue: "payment",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        basicProperties.Headers = new Dictionary<string, object>()
        {
            { "pspRetryCount", 0 },
            { "isPaymentUpdated", false }
        };

        var paymentToQueue = new OutPaymentQueueDto(payment, expireAt);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(paymentToQueue));

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "payment",
            basicProperties: basicProperties,
            body: body);

        Console.WriteLine($"* Sent payment {paymentToQueue.PaymentId} to Payment Queue");
    }
}