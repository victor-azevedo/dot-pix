using DotPixApi.Data;
using DotPixApi.Exceptions;
using DotPixApi.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DotPixApi.Repositories;

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

    public async Task<PixKey?> FindByValueIncludeAccount(string value)
    {
        var key = await context.PixKeys
            .Include(pk => pk.PaymentProviderAccount)
            .ThenInclude(ac => ac.PaymentProvider)
            .Include(pk => pk.PaymentProviderAccount)
            .ThenInclude(ac => ac.User)
            .FirstOrDefaultAsync(
                pixKey => pixKey.Value == value);

        return key;
    }

    public async Task<PixKey?> FindByTypeAndValue(string value)
    {
        var key = await context.PixKeys.FirstOrDefaultAsync(
            pk => pk.Value == value);
        return key;
    }
}