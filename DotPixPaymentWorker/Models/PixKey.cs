using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotPixPaymentWorker.Models;

[Table("pix_keys")]
[Index("PaymentProviderAccountId", Name = "IX_pix_keys_payment_provider_account_id")]
[Index("Value", Name = "IX_pix_keys_value", IsUnique = true)]
public partial class PixKey
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("type")]
    [StringLength(10)]
    public string Type { get; set; } = null!;

    [Column("value")]
    [StringLength(255)]
    public string Value { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("payment_provider_account_id")]
    public int PaymentProviderAccountId { get; set; }

    [ForeignKey("PaymentProviderAccountId")]
    [InverseProperty("PixKeys")]
    public virtual PaymentProviderAccount PaymentProviderAccount { get; set; } = null!;

    [InverseProperty("PixKeyDestiny")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}