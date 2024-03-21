using System.Text;
using System.Text.Json;
using DotPixConciliationWorker.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Services;

public class ConciliationMessageProcessor(ILogger<ConciliationMessageProcessor> logger)
    : IMessageProcessor<ConciliationMessageProcessor>
{
    public async Task ProcessMessageAsync(object model, BasicDeliverEventArgs ea, IModel channel)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        try
        {
            // TODO: Process conciliation...

            logger.LogInformation(message);
            channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (JsonException e)
        {
            logger.LogError(e.Message);
            channel.BasicReject(ea.DeliveryTag, false);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            logger.LogError("\tConciliation not processed");
            channel.BasicReject(ea.DeliveryTag, false);
        }
    }
}