using DotPixWorker.Models;
using Microsoft.EntityFrameworkCore;

namespace DotPixWorker.Data;

public partial class WorkerDbContext(DbContextOptions<WorkerDbContext> options) : DbContext(options)
{
    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentProvider> PaymentProviders { get; set; }

    public virtual DbSet<PaymentProviderAccount> PaymentProviderAccounts { get; set; }

    public virtual DbSet<PaymentProviderToken> PaymentProviderTokens { get; set; }

    public virtual DbSet<PixKey> PixKeys { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

        modelBuilder.Entity<PaymentProviderAccount>(entity =>
        {
            entity.HasOne(d => d.PaymentProvider).WithMany(p => p.PaymentProviderAccounts)
                .HasConstraintName("FK_payment_provider_accounts_payment_providers_payment_provide~");
        });

        modelBuilder.Entity<PaymentProviderToken>(entity =>
        {
            entity.HasOne(d => d.PaymentProvider).WithOne(p => p.PaymentProviderToken)
                .HasConstraintName("FK_payment_provider_tokens_payment_providers_payment_provider_~");
        });

        modelBuilder.Entity<PixKey>(entity =>
        {
            entity.HasOne(d => d.PaymentProviderAccount).WithMany(p => p.PixKeys)
                .HasConstraintName("FK_pix_keys_payment_provider_accounts_payment_provider_account~");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}