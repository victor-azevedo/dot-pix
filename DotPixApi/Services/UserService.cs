using DotPixApi.Exceptions;
using DotPixApi.Models;
using DotPixApi.Repositories;

namespace DotPixApi.Services;

public class UserService(UserRepository userRepository)
{
    public async Task<User> FindByCpf(string cpf)
    {
        var user = await userRepository.FindByCpf(cpf);
        if (user == null)
            throw new CpfNotFoundException($"User CPF: {cpf} not found. Please check and try again.");

        return user;
    }
}