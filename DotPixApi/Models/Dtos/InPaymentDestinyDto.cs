using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InPaymentDestinyDto
{
    [Required(ErrorMessage = "Field 'key' is required")]
    public InPixKeyDto Key { get; set; }
}