using DotPixWorker.Models.Dtos;

namespace DotPixWorker.Interfaces;

public interface IPaymentRepository
{
    Task<bool> UpdatePaymentStatus(InPaymentQueueDto payment);
}