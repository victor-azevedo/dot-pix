using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InPostPaymentDto
{
    [Required(ErrorMessage = "Field 'origin' is required")]
    public required InPaymentOriginDto Origin { get; set; }

    [Required(ErrorMessage = "Field 'destiny' is required")]
    public required InPaymentDestinyDto Destiny { get; set; }

    [Required(ErrorMessage = "Field 'amount' is required")]
    public required int Amount { get; set; }

    public string? Description { get; set; }
}