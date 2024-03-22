using DotPixApi.Interfaces;
using DotPixApi.Models.Dtos;

namespace DotPixApi.Services;

public class PaymentQueuePublisher(IQueuePublisherService queuePublisherService)
{
    private const string QUEUE_NAME = "payment";

    public void PublishMessage(OutPaymentQueueDto paymentDto)
    {
        var headers = new Dictionary<string, object>
        {
            { "pspRetryCount", 0 },
            { "isPaymentUpdated", false }
        };

        queuePublisherService.PublishMessage(QUEUE_NAME, paymentDto, headers);
    }
}