using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Interfaces;

public interface IQueueExceptions<T>
{
    void HandleException(Exception e, BasicDeliverEventArgs ea);
}