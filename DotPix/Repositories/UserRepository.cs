using DotPix.Data;
using DotPix.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPix.Repositories;

public class UserRepository(AppDbContext context)
{
    public async Task<User?> FindByCpf(string cpf)
    {
        return await context.User.SingleOrDefaultAsync(u => u.Cpf == cpf);
    }
}