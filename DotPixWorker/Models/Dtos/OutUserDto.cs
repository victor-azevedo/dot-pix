namespace DotPixWorker.Models.Dtos;

public class OutUserDto(InUserDto user)
{
    public string Name { get; set; } = user.Name;
    public string MaskedCpf { get; set; } = user.MaskedCpf;
}