using DotPix.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPix.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<PaymentProvider> PaymentProvider { get; set; }
    public DbSet<PaymentProviderToken> PaymentProviderToken { get; set; }
    public DbSet<PaymentProviderAccount> PaymentProviderAccount { get; set; }
    public DbSet<PixKey> PixKeys { get; set; }

    public DbSet<Payments> Payments { get; set; }
}