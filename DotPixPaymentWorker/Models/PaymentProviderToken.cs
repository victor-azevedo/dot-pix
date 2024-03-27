using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotPixPaymentWorker.Models;

[Table("payment_provider_tokens")]
[Index("PaymentProviderId", Name = "IX_payment_provider_tokens_payment_provider_id", IsUnique = true)]
[Index("Token", Name = "IX_payment_provider_tokens_token", IsUnique = true)]
public partial class PaymentProviderToken
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("token")]
    [StringLength(255)]
    public string Token { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("payment_provider_id")]
    public int PaymentProviderId { get; set; }

    [ForeignKey("PaymentProviderId")]
    [InverseProperty("PaymentProviderToken")]
    public virtual PaymentProvider PaymentProvider { get; set; } = null!;
}