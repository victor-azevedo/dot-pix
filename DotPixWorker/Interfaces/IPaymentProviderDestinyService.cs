using DotPixWorker.Models;
using DotPixWorker.Models.Dtos;

namespace DotPixWorker.Interfaces;

public interface IPaymentProviderDestinyService
{
    public Task<bool> HandlePaymentToDestiny(InPaymentQueueDto payment);
}