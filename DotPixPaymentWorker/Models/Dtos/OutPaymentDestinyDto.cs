namespace DotPixPaymentWorker.Models.Dtos;

public class OutPaymentDestinyDto(InPaymentDestinyDto paymentDestiny)
{
    public OutPixKeyDto Key { get; set; } = new(paymentDestiny.Key);
}