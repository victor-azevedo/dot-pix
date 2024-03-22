using DotPixApi.Models.Dtos;

namespace DotPixApi.Services;

public class ConciliationService(
    HttpContextService httpContextService,
    ConciliationQueuePublisher conciliationQueuePublisher)
{
    public void SendConciliationToWorker(InPostConciliationDto inPostConciliationDto)
    {
        var paymentProviderId = httpContextService.GetPaymentProviderIdFromHttpContext();

        var conciliationDto = new OutConciliationQueueDto(paymentProviderId, inPostConciliationDto);

        conciliationQueuePublisher.PublishMessage(conciliationDto);
    }
}