using DotPix.Data;
using DotPix.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPix.Repositories;

public class PaymentProviderTokenRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task<PaymentProviderToken?> FindPaymentProviderByToken(string token)
    {
        var paymentProviderToken = await _context.PaymentProviderToken.SingleOrDefaultAsync(p => p.Token == token);

        return paymentProviderToken;
    }
}