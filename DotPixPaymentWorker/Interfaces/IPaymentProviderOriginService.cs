using DotPixPaymentWorker.Models.Dtos;

namespace DotPixPaymentWorker.Interfaces;

public interface IPaymentProviderOriginService
{
    public Task HandlePaymentToOrigin(InPaymentQueueDto payment);
}