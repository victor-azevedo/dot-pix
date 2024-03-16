using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPixApi.Models;

[Table("payment_providers")]
public class PaymentProvider(string name, string apiUrl)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = name;

    [Required]
    [Column("api_url", TypeName = "varchar(255)")]
    public string ApiUrl { get; set; } = apiUrl;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public PaymentProviderToken? Token { get; set; }

    public ICollection<PaymentProviderAccount> PaymentProviderAccount { get; } = new List<PaymentProviderAccount>();
}