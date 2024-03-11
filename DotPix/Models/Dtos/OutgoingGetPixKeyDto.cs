namespace DotPix.Models.Dtos;

public class OutgoingGetPixKeyDto(PixKey pixKey)
{
    public GetPixKeyDto Key { get; set; } = new(pixKey);
    public GetPixKeyUserDto User { get; set; } = new(pixKey.PaymentProviderAccount.User);
    public GetPixKeyAccountDto Account { get; set; } = new(pixKey.PaymentProviderAccount);
}