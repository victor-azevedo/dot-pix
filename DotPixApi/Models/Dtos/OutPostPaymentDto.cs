namespace DotPixApi.Models.Dtos;

public class OutPostPaymentDto(Payments payment)
{
    public Guid PaymentId { get; set; } = payment.Id;
    public int Amount { get; set; } = payment.Amount;
    public string Status { get; set; } = payment.Status.ToString();
    public string? Description { get; set; } = payment.Description;
    public OutPaymentOriginDto Origin { get; set; } = new(payment.AccountOrigin);
    public OutPaymentDestinyDto Destiny { get; set; } = new(payment.PixKeyDestiny);
}