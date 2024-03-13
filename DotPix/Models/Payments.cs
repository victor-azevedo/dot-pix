using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPix.Models;

public enum PaymentStatus
{
    SUCCESS,
    PROCESSING,
    FAILED
}

[Table("payments")]
public class Payments(Guid uudi, int amount, string? description)
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = uudi;

    [Required]
    [Column("amount")]
    public int Amount { get; set; } = amount;

    [Column("description", TypeName = "varchar(255)")]
    public string? Description { get; set; } = description;

    [Column("status", TypeName = "varchar(255)")]
    public PaymentStatus Status { get; set; } = PaymentStatus.PROCESSING;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("account_origin_id")]
    public int AccountOriginId { get; set; }

    public PaymentProviderAccount AccountOrigin { get; set; }

    [Required]
    [Column("pix_key_destiny_id")]
    public int PixKeyDestinyId { get; set; }

    public PixKey PixKeyDestiny { get; set; }
}