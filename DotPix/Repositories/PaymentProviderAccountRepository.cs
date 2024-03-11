using DotPix.Data;
using DotPix.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPix.Repositories;

public class PaymentProviderAccountRepository(AppDbContext context)
{
    public async Task<List<PaymentProviderAccount>> FindByUser(User user)
    {
        var userAccounts = await context.PaymentProviderAccount
            .Where(account => account.UserId == user.Id)
            .Include(account => account.PixKey)
            .ToListAsync();

        return userAccounts;
    }
}