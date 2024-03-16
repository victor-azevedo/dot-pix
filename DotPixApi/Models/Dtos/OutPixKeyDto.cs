namespace DotPixApi.Models.Dtos;

public class OutPixKeyDto(PixKey key)
{
    public string Value { get; set; } = key.Value;
    public string Type { get; set; } = key.Type.ToString();
}