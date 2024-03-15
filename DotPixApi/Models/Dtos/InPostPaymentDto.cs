using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InPostPaymentDto
{
    [Required(ErrorMessage = "Field 'origin' is required")]
    public InPaymentOriginDto Origin { get; set; }

    [Required(ErrorMessage = "Field 'destiny' is required")]
    public InPaymentDestinyDto Destiny { get; set; }

    [Required(ErrorMessage = "Field 'amount' is required")]
    public int Amount { get; set; }

    public string? Description { get; set; }
}