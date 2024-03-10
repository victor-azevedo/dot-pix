using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DotPix.Models;

[Table("payment_provider_tokens")]
[Index(nameof(Token), IsUnique = true)]
public class PaymentProviderToken(string token)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("token", TypeName = "varchar(255)")]
    public string Token { get; set; } = token;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("payment_provider_id")]
    public int PaymentProviderId { get; set; }

    public PaymentProvider PaymentProvider { get; set; }
}