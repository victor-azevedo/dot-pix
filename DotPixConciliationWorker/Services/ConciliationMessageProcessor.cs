using System.Text;
using System.Text.Json;
using DotPixConciliationWorker.Exceptions;
using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Models.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixConciliationWorker.Services;

public class ConciliationMessageProcessor(
    IConciliationFileProcessor conciliationFileProcessor,
    IPspClient pspClient,
    IQueueExceptions<ConciliationQueueExceptions> conciliationQueueExceptions,
    ILogger<ConciliationMessageProcessor> logger)
    : IMessageProcessor<ConciliationMessageProcessor>
{
    public async Task ProcessMessageAsync(object model, BasicDeliverEventArgs ea, IModel channel)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        try
        {
            var conciliation = JsonSerializer.Deserialize<InConciliationQueueDto>(message);
            if (conciliation is null)
                throw new JsonException();

            logger.LogInformation(conciliation.Date.ToShortDateString());
            logger.LogInformation(conciliation.PaymentProviderId.ToString());

            var conciliationBalance = await conciliationFileProcessor.GetConciliationBalance(conciliation);

            await pspClient.PostConciliation(conciliation.Postback, conciliationBalance);
            
            channel.BasicAck(ea.DeliveryTag, false);
            logger.LogInformation("Conciliation balance processed successfully!"); 
        }
        catch (Exception e)
        {
            conciliationQueueExceptions.HandleException(e, ea);
        }
    }
}