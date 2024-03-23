using System.Text.Json;
using DotPixConciliationWorker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Exceptions;

public class ConciliationQueueExceptions(
    ILogger<ConciliationQueueExceptions> logger,
    IModel channel)
    :IQueueExceptions<ConciliationQueueExceptions>
{
    public void HandleException(Exception e, BasicDeliverEventArgs ea)
    {
        switch (e)
        {
            case JsonException jsonException:
                HandleJsonException(jsonException, ea);
                break;
            case HttpRequestException httpRequestException:
                HandleHttpRequestException(httpRequestException, ea);
                break;
            default:
                HandleGenericException(e, ea);
                break;
        }
    }

    private void HandleJsonException(JsonException e, BasicDeliverEventArgs ea)
    {
        logger.LogError(e, e.Message);
        logger.LogError("Unexpected message format");
        channel.BasicReject(ea.DeliveryTag, false);
    }

    private void HandleHttpRequestException(HttpRequestException e, BasicDeliverEventArgs ea)
    {
        logger.LogError(e, e.Message);
        logger.LogError("PSP Postback unsuccessful");
        channel.BasicReject(ea.DeliveryTag, false);
    }

    private void HandleGenericException(Exception e, BasicDeliverEventArgs ea)
    {
        logger.LogError(e, "An unexpected error occurred");
        channel.BasicReject(ea.DeliveryTag, false);
    }
}