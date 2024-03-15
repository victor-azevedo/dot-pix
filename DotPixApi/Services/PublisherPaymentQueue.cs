using System.Text;
using System.Text.Json;
using DotPixApi.Models.Dtos;
using RabbitMQ.Client;

namespace DotPixApi.Services;

public class PublisherPaymentQueue
{
    public void Send(OutPostPaymentDto payment)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "payment",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payment));

        channel.BasicPublish(exchange: string.Empty,
            routingKey: "payment",
            basicProperties: null,
            body: body);

        Console.WriteLine($" [x] Sent");
    }
}