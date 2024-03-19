using DotPixPaymentWorker.Data;
using DotPixPaymentWorker.Interfaces;
using DotPixPaymentWorker.Models;
using DotPixPaymentWorker.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DotPixPaymentWorker.Repositories;

public class PaymentRepository(IDbContextFactory<DotPixDbContext> dbContextFactory) : IPaymentRepository
{
    public async Task<bool> UpdatePaymentStatus(InPaymentQueueDto payment)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();

        var paymentIdParameter = new Npgsql.NpgsqlParameter("@paymentId", payment.PaymentId);
        var statusParameter = new Npgsql.NpgsqlParameter("@status", payment.Status.ToString());

        var rowsAffected = await dbContext.Database.ExecuteSqlRawAsync(
            "UPDATE Payments SET Status = @status WHERE Id = @paymentId",
            paymentIdParameter, statusParameter);

        return rowsAffected == 1;
    }
}