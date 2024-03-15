namespace DotPixApi.Models.Dtos;

public class OutPayOriginDto(PaymentProviderAccount account)
{
    public OutUserDto User { get; set; } = new(account.User);
    public OutAccountDto Account { get; set; } = new(account);
}