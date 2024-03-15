namespace DotPixApi.Models.Dtos;

public class OutPixKeyDto(PixKey key)
{
    public string value { get; set; } = key.Value;
    public string type { get; set; } = key.Type.ToString();
}