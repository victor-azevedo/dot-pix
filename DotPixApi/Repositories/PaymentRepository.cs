using DotPixApi.Data;
using DotPixApi.Models;
using DotPixApi.Models.IdempotencyKeys;
using Microsoft.EntityFrameworkCore;

namespace DotPixApi.Repositories;

public class PaymentRepository(AppDbContext context)
{
    public async Task Create(Payments payment)
    {
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();
    }

    public async Task<Payments?> FindByIdempotencyKeyAndTimeTolerance(
        PaymentIdempotencyKey idempotencyKey, DateTime timeTolerance)
    {
        var recentPayment = await context.Payments
            .FirstOrDefaultAsync(p =>
                p.AccountOriginId == idempotencyKey.AccountOriginId &&
                p.PixKeyDestinyId == idempotencyKey.PixKeyDestinyId &&
                p.Amount == idempotencyKey.Amount &&
                p.CreatedAt >= timeTolerance);

        return recentPayment;
    }
}