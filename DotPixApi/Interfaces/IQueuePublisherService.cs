namespace DotPixApi.Interfaces;

public interface IQueuePublisherService
{
    void PublishMessage<T>(string queueName, T messageObj, Dictionary<string, object>? headers);
}