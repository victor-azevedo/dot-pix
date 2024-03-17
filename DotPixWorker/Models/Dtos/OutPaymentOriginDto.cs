namespace DotPixWorker.Models.Dtos;

public class OutPaymentOriginDto(InPaymentOriginDto paymentOrigin)
{
    public OutUserDto User { get; set; } = new(paymentOrigin.User);
    public OutAccountDto Account { get; set; } = new(paymentOrigin.Account);
}