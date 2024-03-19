namespace DotPixPaymentWorker.Models.Dtos;

public class OutPixKeyDto(InPixKeyDto key)
{
    public string value { get; set; } = key.Value;
    public string type { get; set; } = key.Type;
}