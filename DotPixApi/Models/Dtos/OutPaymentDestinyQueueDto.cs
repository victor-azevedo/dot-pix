namespace DotPixApi.Models.Dtos;

public class OutPaymentDestinyQueueDto(PixKey pixKey)
{
    public OutPixKeyDto Key { get; set; } = new(pixKey);
    public int AccountDestinyId { get; set; } = pixKey.PaymentProviderAccountId;
}