using DotPixApi.Data;
using DotPixApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPixApi.Repositories;

public class UserRepository(AppDbContext context)
{
    public async Task<User?> FindByCpf(string cpf)
    {
        return await context.User.SingleOrDefaultAsync(u => u.Cpf == cpf);
    }
}