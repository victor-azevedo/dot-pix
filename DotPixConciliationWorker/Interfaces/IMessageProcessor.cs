using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Interfaces;

public interface IMessageProcessor<T>
{
    Task ProcessMessageAsync(object model, BasicDeliverEventArgs ea, IModel channel);
}