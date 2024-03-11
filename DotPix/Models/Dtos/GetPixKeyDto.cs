namespace DotPix.Models.Dtos;

public class GetPixKeyDto(PixKey key)
{
    public string value { get; set; } = key.Value;
    public string type { get; set; } = key.Type.ToString();
}