using DotPix.Data;
using DotPix.Exceptions;
using DotPix.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;

namespace DotPix.Repositories;

public class PaymentRepository(AppDbContext context)
{
    public async Task Create(Payments payment)
    {
        await context.Payments.AddAsync(payment);
        await context.SaveChangesAsync();
    }
}