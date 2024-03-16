using DotPixWorker.Models;
using DotPixWorker.Models.Dtos;

namespace DotPixWorker.Interfaces;

public interface IPaymentProviderDestinyService
{
    public Task HandlePaymentToDestiny(InPaymentQueueDto payment);
}