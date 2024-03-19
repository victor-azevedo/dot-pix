using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotPixPaymentWorker.Models;

[Table("payment_provider_accounts")]
[Index("PaymentProviderId", Name = "IX_payment_provider_accounts_payment_provider_id")]
[Index("UserId", Name = "IX_payment_provider_accounts_user_id")]
public partial class PaymentProviderAccount
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("account")]
    [StringLength(20)]
    public string Account { get; set; } = null!;

    [Column("agency")]
    [StringLength(20)]
    public string Agency { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("payment_provider_id")]
    public int PaymentProviderId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("PaymentProviderId")]
    [InverseProperty("PaymentProviderAccounts")]
    public virtual PaymentProvider PaymentProvider { get; set; } = null!;

    [InverseProperty("AccountOrigin")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("PaymentProviderAccount")]
    public virtual ICollection<PixKey> PixKeys { get; set; } = new List<PixKey>();

    [ForeignKey("UserId")]
    [InverseProperty("PaymentProviderAccounts")]
    public virtual User User { get; set; } = null!;
}