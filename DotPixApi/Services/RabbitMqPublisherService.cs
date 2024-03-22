using System.Text;
using System.Text.Json;
using DotPixApi.Interfaces;
using RabbitMQ.Client;

namespace DotPixApi.Services;

public class RabbitMqPublisherService(IConnectionFactory factory) : IQueuePublisherService
{
    public void PublishMessage<T>(string queueName, T messageObj)
    {
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var basicProperties = channel.CreateBasicProperties();
        basicProperties.Persistent = true;

        DeclareQueue(channel, queueName);

        var message = JsonSerializer.Serialize(messageObj);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queueName,
            basicProperties: basicProperties,
            body: body);

        Console.WriteLine($"Message published to queue '{queueName}': {message}");
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