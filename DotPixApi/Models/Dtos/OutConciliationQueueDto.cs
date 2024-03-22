namespace DotPixApi.Models.Dtos;

public class OutConciliationQueueDto(int paymentProviderId, InPostConciliationDto conciliationDto)
{
    public int PaymentProviderId { get; set; } = paymentProviderId;
    public DateOnly Date { get; set; } = DateOnly.Parse(conciliationDto.Date);
    public string File { get; set; } = conciliationDto.File;
    public string Postback { get; set; } = conciliationDto.Postback;
}