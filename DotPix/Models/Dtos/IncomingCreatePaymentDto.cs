using System.ComponentModel.DataAnnotations;

namespace DotPix.Models.Dtos;

public class IncomingCreatePaymentDto
{
    [Required(ErrorMessage = "Field 'origin' is required")]
    public CreatePaymentOriginDto Origin { get; set; }

    [Required(ErrorMessage = "Field 'destiny' is required")]
    public CreatePaymentDestinyDto Destiny { get; set; }

    [Required(ErrorMessage = "Field 'amount' is required")]
    public int Amount { get; set; }

    public string? Description { get; set; }
}