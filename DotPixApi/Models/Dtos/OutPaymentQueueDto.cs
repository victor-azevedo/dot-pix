namespace DotPixApi.Models.Dtos;

public class OutPaymentQueueDto(Payments payment)
{
    public Guid PaymentId { get; set; } = payment.Id;
    public int Amount { get; set; } = payment.Amount;
    public string? Description { get; set; } = payment.Description;
    public OutPaymentOriginDto Origin { get; set; } = new(payment.AccountOrigin);
    public OutPaymentDestinyQueueDto Destiny { get; set; } = new(payment.PixKeyDestiny);
}