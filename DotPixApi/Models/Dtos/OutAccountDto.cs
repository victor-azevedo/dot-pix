namespace DotPixApi.Models.Dtos;

public class OutAccountDto(PaymentProviderAccount paymentProviderAccount)
{
    public string Number { get; set; } = paymentProviderAccount.Account;
    public string Agency { get; set; } = paymentProviderAccount.Agency;
    public string BankName { get; set; } = paymentProviderAccount.PaymentProvider.Name;
}