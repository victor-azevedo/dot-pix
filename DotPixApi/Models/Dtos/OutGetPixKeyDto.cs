namespace DotPixApi.Models.Dtos;

public class OutGetPixKeyDto(PixKey pixKey)
{
    public OutPixKeyDto Key { get; set; } = new(pixKey);
    public OutUserDto User { get; set; } = new(pixKey.PaymentProviderAccount.User);
    public OutAccountDto Account { get; set; } = new(pixKey.PaymentProviderAccount);
}