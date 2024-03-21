using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotPixConciliationWorker.Models;

[Table("payments")]
[Index("AccountOriginId", Name = "IX_payments_account_origin_id")]
[Index("PixKeyDestinyId", Name = "IX_payments_pix_key_destiny_id")]
public partial class Payment
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("status")]
    [StringLength(255)]
    public string Status { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("account_origin_id")]
    public int AccountOriginId { get; set; }

    [Column("pix_key_destiny_id")]
    public int PixKeyDestinyId { get; set; }

    [ForeignKey("AccountOriginId")]
    [InverseProperty("Payments")]
    public virtual PaymentProviderAccount AccountOrigin { get; set; } = null!;

    [ForeignKey("PixKeyDestinyId")]
    [InverseProperty("Payments")]
    public virtual PixKey PixKeyDestiny { get; set; } = null!;
}
