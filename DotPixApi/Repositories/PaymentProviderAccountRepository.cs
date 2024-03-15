using DotPixApi.Data;
using DotPixApi.Models;
using DotPixApi.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DotPixApi.Repositories;

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

    public async Task<PaymentProviderAccount?> FindByUserAndAccount(
        User user, int paymentProviderId, InAccountDto account)
    {
        var userAccount = await context.PaymentProviderAccount.FirstOrDefaultAsync(acc =>
            acc.UserId == user.Id &&
            acc.PaymentProviderId == paymentProviderId &&
            acc.Account == account.Number &&
            acc.Agency == account.Agency);

        return userAccount;
    }
}