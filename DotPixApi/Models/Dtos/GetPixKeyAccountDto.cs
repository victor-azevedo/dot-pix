namespace DotPixApi.Models.Dtos;

public class GetPixKeyAccountDto(PaymentProviderAccount paymentProviderAccount)
{
    public string Number { get; set; } = paymentProviderAccount.Account;
    public string Agency { get; set; } = paymentProviderAccount.Agency;
    public string BankName { get; set; } = paymentProviderAccount.PaymentProvider.Name;
    public string BankId { get; set; } = paymentProviderAccount.PaymentProviderId.ToString();
}