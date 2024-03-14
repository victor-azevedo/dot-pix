using DotPixApi.Data;
using DotPixApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPixApi.Repositories;

public class PaymentProviderTokenRepository(AppDbContext context)
{
    public async Task<PaymentProviderToken?> FindByToken(string token)
    {
        var paymentProviderToken = await context.PaymentProviderToken.SingleOrDefaultAsync(p => p.Token == token);

        return paymentProviderToken;
    }
}