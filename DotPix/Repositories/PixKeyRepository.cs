using DotPix.Data;
using DotPix.Exceptions;
using DotPix.Models;
using DotPix.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DotPix.Repositories;

public class PixKeyRepository(AppDbContext context)
{
    public async Task Create(PixKey pixKey)
    {
        try
        {
            await context.PixKeys.AddAsync(pixKey);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            Console.WriteLine(ex);
            throw new ConflictException("The key value is already in use");
        }
    }
}