using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixPaymentWorker.Interfaces;

public interface IMessageProcessor
{
    Task ProcessMessageAsync(object model, BasicDeliverEventArgs ea, IModel channel);
}