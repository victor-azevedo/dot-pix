using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPix.Models;

[Table("payment_provider_accounts")]
public class PaymentProviderAccount(string account, string agency)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("account", TypeName = "varchar(20)")]
    public string Account { get; set; } = account;

    [Required]
    [Column("agency", TypeName = "varchar(20)")]
    public string Agency { get; set; } = agency;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("payment_provider_id")]
    public int PaymentProviderId { get; set; }

    public PaymentProvider PaymentProvider { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    public User User { get; set; }

    public ICollection<PixKey> PixKey { get; } = new List<PixKey>();
}