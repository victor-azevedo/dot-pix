namespace DotPixApi.Models.Dtos;

public class OutPaymentOriginToQueueDto(PaymentProviderAccount account)
{
    public OutUserDto User { get; set; } = new(account.User);
    public OutAccountToQueueDto Account { get; set; } = new(account);
}