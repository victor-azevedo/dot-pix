using DotPixWorker.Models;
using DotPixWorker.Models.Dtos;

namespace DotPixWorker.Interfaces;

public interface IPaymentProviderOriginService
{
    public Task HandlePaymentToOrigin(InPaymentQueueDto payment);
}