namespace DotPixWorker.Models.Dtos;

public class InPaymentQueueDto
{
    public required Guid PaymentId { get; set; }
    public required int Amount { get; set; }
    public required string? Description { get; set; }
    public required InPaymentOriginDto Origin { get; set; }
    public required InPaymentDestinyDto Destiny { get; set; }
    public required PaymentStatus Status { get; set; }
    public required DateTime ExpireAt { get; set; }
}