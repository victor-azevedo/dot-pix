namespace DotPixPaymentWorker.Models.Dtos;

public class OutAccountDto(InAccountDto account)
{
    public string Number { get; set; } = account.Number;
    public string Agency { get; set; } = account.Agency;
    public string BankName { get; set; } = account.BankName;
}