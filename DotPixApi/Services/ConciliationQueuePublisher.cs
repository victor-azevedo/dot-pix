using DotPixApi.Interfaces;
using DotPixApi.Models.Dtos;

namespace DotPixApi.Services;

public class ConciliationQueuePublisher(IQueuePublisherService queuePublisherService)
{
    private const string QUEUE_NAME = "conciliation";

    public void PublishMessage(OutConciliationQueueDto conciliationDto)
    {
        queuePublisherService.PublishMessage(QUEUE_NAME, conciliationDto);
    }
}