namespace DotPixApi.Models.Dtos;

public class OutPaymentDestinyDto(PixKey pixKey)
{
    public OutPixKeyDto Key { get; set; } = new(pixKey);
}