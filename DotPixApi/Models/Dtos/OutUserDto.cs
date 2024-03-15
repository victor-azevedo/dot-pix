namespace DotPixApi.Models.Dtos;

public class OutUserDto(User user)
{
    public string Name { get; set; } = user.Name;
    public string MaskedCpf { get; set; } = GetMaskedCpf(user.Cpf);

    private static string GetMaskedCpf(string cpf)
    {
        return $"{cpf.Substring(0, 3)}.XXX.XXX-{cpf.Substring(9)}";
    }
}