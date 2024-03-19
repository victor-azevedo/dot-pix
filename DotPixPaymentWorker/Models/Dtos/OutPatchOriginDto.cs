namespace DotPixPaymentWorker.Models.Dtos;

public class OutPatchOriginDto(InPaymentQueueDto payment)
{
    public Guid PaymentId { get; set; } = payment.PaymentId;
    public string Status { get; set; } = payment.Status.ToString();
}