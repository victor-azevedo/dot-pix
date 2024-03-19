using DotPixPaymentWorker.Models.Dtos;

namespace DotPixPaymentWorker.Interfaces;

public interface IPaymentRepository
{
    Task<bool> UpdatePaymentStatus(InPaymentQueueDto payment);
}