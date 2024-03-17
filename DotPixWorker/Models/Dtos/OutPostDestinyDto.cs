namespace DotPixWorker.Models.Dtos;

public class OutPostDestinyDto(InPaymentQueueDto payment)
{
    public Guid PaymentId { get; set; } = payment.PaymentId;
    public int Amount { get; set; } = payment.Amount;
    public string? Description { get; set; } = payment.Description;
    public OutPaymentOriginDto Origin { get; set; } = new(payment.Origin);
    public OutPaymentDestinyDto Destiny { get; set; } = new(payment.Destiny);
}