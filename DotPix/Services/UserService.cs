using DotPix.Exceptions;
using DotPix.Models;
using DotPix.Repositories;

namespace DotPix.Services;

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