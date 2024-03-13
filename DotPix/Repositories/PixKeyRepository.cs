using DotPix.Data;
using DotPix.Exceptions;
using DotPix.Models;
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

    public async Task<PixKey> FindByTypeAndValueIncludeAccount(PixKeyTypes pixKeyType, string value)
    {
        var key = await context.PixKeys
            .Include(pk => pk.PaymentProviderAccount)
            .ThenInclude(ac => ac.PaymentProvider)
            .Include(pk => pk.PaymentProviderAccount)
            .ThenInclude(ac => ac.User)
            .FirstOrDefaultAsync(
                pixKey => pixKey.Type == pixKeyType && pixKey.Value == value);

        if (key == null)
            throw new PixKeyNotFoundException();

        return key;
    }

    public async Task<PixKey?> FindByTypeAndValue(PixKeyTypes pixKeyType, string value)
    {
        var key = await context.PixKeys.FirstOrDefaultAsync(
            pk => pk.Type == pixKeyType && pk.Value == value);
        return key;
    }
}