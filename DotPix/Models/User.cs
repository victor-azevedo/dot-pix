using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotPix.Models;

[Table("users")]
public class User
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }
    
    [Required]
    [Column("name",TypeName = "varchar(255)")]
    public string Name { get; set; }

    [Required]
    [Column("cpf",TypeName = "varchar(11)")]
    public string Cpf { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; init; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public User(string name, string cpf)
    {
        Name = name;
        Cpf = cpf;
        CreatedAt = UpdatedAt = DateTime.UtcNow;
    }
}