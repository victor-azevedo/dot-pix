namespace DotPixApi.Models.Dtos;

public class OutAccountDto(PaymentProviderAccount paymentProviderAccount)
{
    public string Number { get; set; } = paymentProviderAccount.Account;
    public string Agency { get; set; } = paymentProviderAccount.Agency;
    public string BankId { get; set; } = paymentProviderAccount.PaymentProviderId.ToString();
}