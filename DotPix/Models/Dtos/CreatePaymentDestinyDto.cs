using System.ComponentModel.DataAnnotations;

namespace DotPix.Models.Dtos;

public class CreatePaymentDestinyDto
{
    [Required(ErrorMessage = "Field 'key' is required")]
    public PostPixKeyDto Key { get; set; }
}