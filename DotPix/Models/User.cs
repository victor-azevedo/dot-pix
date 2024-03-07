using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPix.Models;

[Table("users")]
public class User(string name, string cpf)
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(255)")]
    public string Name { get; set; } = name;

    [Required]
    [Column("cpf", TypeName = "varchar(11)")]
    public string Cpf { get; set; } = cpf;

    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PaymentProviderAccount> PaymentProviderAccount { get; } = new List<PaymentProviderAccount>();
}