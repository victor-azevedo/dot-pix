using DotPixApi.Data;
using DotPixApi.Exceptions;
using DotPixApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;

namespace DotPixApi.Repositories;

public class PaymentRepository(AppDbContext context)
{
    public async Task Create(Payments payment)
    {
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();
    }
}