﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPixConciliationWorker.Models;

[Table("payment_providers")]
public partial class PaymentProvider
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("api_url")]
    [StringLength(255)]
    public string ApiUrl { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("PaymentProvider")]
    public virtual ICollection<PaymentProviderAccount> PaymentProviderAccounts { get; set; } = new List<PaymentProviderAccount>();

    [InverseProperty("PaymentProvider")]
    public virtual PaymentProviderToken? PaymentProviderToken { get; set; }
}
