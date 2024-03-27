using System.Text;
using System.Text.Json;
using DotPixPaymentWorker.Interfaces;
using DotPixPaymentWorker.Models;
using DotPixPaymentWorker.Models.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotPixPaymentWorker.Services;

public class PaymentMessageProcessor(
    IPaymentProviderDestinyService paymentProviderDestiny,
    IPaymentProviderOriginService paymentProviderOrigin,
    IPaymentRepository paymentRepository,
    ILogger<PaymentMessageProcessor> logger)
    : IMessageProcessor
{
    private const int MAX_REQUEUE = 3;

    public async Task ProcessMessageAsync(object model, BasicDeliverEventArgs ea, IModel channel)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        int pspRetryCountHeader = 0;
        bool isPaymentUpdatedHeader = false;

        InPaymentQueueDto? payment = null;
        try
        {
            payment = GetPaymentFromMessageOrThrow(message);

            pspRetryCountHeader =
                Int32.Parse(ea.BasicProperties.Headers["pspRetryCount"].ToString() ?? string.Empty);

            isPaymentUpdatedHeader =
                bool.Parse(ea.BasicProperties.Headers["isPaymentUpdated"].ToString() ?? string.Empty);

            if (pspRetryCountHeader > MAX_REQUEUE)
                throw new TaskCanceledException("Maximum connection retry attempts to PSP reached.");

            logger.LogInformation($"--> Received payment: '{payment.PaymentId}'");

            if (payment.Status == PaymentStatus.PROCESSING)
            {
                var isPaymentProcessingSuccess = await paymentProviderDestiny.HandlePaymentToDestiny(payment);
                payment.Status = isPaymentProcessingSuccess ? PaymentStatus.SUCCESS : PaymentStatus.FAILED;

                logger.LogInformation(
                    $"\t - Payment ID:'{payment.PaymentId}' sent to PSP Destiny - Status: '{payment.Status}'");
                pspRetryCountHeader = 0;
            }

            if (!isPaymentUpdatedHeader)
            {
                isPaymentUpdatedHeader = await paymentRepository.UpdatePaymentStatus(payment);
                logger.LogInformation(
                    $"\t - Payment ID:'{payment.PaymentId}' - Status: '{payment.Status}' updated in DB: {isPaymentUpdatedHeader}");
            }

            await paymentProviderOrigin.HandlePaymentToOrigin(payment);
            logger.LogInformation(
                $"\t - Payment ID:'{payment.PaymentId}' sent to PSP Origin - Status: '{payment.Status}'");

            channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (JsonException e)
        {
            logger.LogError(e.Message);
            channel.BasicReject(ea.DeliveryTag, false);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e.Message);
            var newHeaders = ea.BasicProperties.Headers;

            newHeaders["pspRetryCount"] = pspRetryCountHeader + 1;
            newHeaders["isPaymentUpdated"] = isPaymentUpdatedHeader;

            var newBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payment));

            if (payment != null)
                logger.LogError($"\t - Payment ID:'{payment.PaymentId}' republishing - Status: '{payment.Status}'");
            channel.BasicReject(ea.DeliveryTag, false);
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "payment",
                basicProperties: ea.BasicProperties,
                body: newBody);
        }
        catch (TaskCanceledException e)
        {
            logger.LogError(e.Message);
            logger.LogError($"\t - Payment ID:'{payment!.PaymentId}' Timeout to PSP");

            payment.Status = PaymentStatus.FAILED;
            isPaymentUpdatedHeader = await paymentRepository.UpdatePaymentStatus(payment);
            logger.LogInformation(
                $"\t - Payment ID:'{payment.PaymentId}' - Status: '{payment.Status}' updated in DB: {isPaymentUpdatedHeader}");

            channel.BasicAck(ea.DeliveryTag, false);

            await paymentProviderOrigin.HandlePaymentToOrigin(payment);
            logger.LogInformation(
                $"\t - Payment ID:'{payment.PaymentId}' sent to PSP Origin - Status: '{payment.Status}'");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            logger.LogError($"\t Payment not processed");
            channel.BasicReject(ea.DeliveryTag, false);
        }
    }

    private InPaymentQueueDto GetPaymentFromMessageOrThrow(string message)
    {
        var payment = JsonSerializer.Deserialize<InPaymentQueueDto>(message);
        if (payment == null)
            throw new JsonException("Invalid message format from Payment Queue");
        return payment;
    }
}