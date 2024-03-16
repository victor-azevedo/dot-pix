namespace DotPixWorker.Models.Dtos;

public class InPaymentQueueDto
{
    public Guid PaymentId { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
    public InPaymentOriginDto Origin { get; set; }
    public InPaymentDestinyDto Destiny { get; set; }
}