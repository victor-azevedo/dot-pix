using DotPixPaymentWorker.Models.Dtos;

namespace DotPixPaymentWorker.Interfaces;

public interface IPaymentProviderDestinyService
{
    public Task<bool> HandlePaymentToDestiny(InPaymentQueueDto payment);
}