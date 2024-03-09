using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPix.Models;

[Table("pix_keys")]
public class PixKey(PixKeyTypes type, string value)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("type", TypeName = "varchar(10)")]
    public PixKeyTypes Type { get; set; } = type;

    [Required]
    [Column("value", TypeName = "varchar(255)")]
    public string Value { get; set; } = value;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("payment_provider_account_id")]
    public int PaymentProviderAccountId { get; set; }

    public PaymentProviderAccount PaymentProviderAccount { get; set; } = null!;
}