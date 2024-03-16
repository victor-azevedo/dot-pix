namespace DotPixApi.Models.IdempotencyKeys;

public class PaymentIdempotencyKey(Payments payment)
{
    public int AccountOriginId { get; set; } = payment.AccountOrigin.Id;
    public int PixKeyDestinyId { get; set; } = payment.PixKeyDestiny.Id;
}