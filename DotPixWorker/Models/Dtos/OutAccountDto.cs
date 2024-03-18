namespace DotPixWorker.Models.Dtos;

public class OutAccountDto(InAccountDto account)
{
    public string Number { get; set; } = account.Number;
    public string Agency { get; set; } = account.Agency;
    public string BankId { get; set; } = account.BankId;
}