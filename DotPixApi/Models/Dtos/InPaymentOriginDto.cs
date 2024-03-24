using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InPaymentOriginDto
{
    [Required(ErrorMessage = "Field 'user' is required")]
    public required InUserDto User { get; set; }

    [Required(ErrorMessage = "Field 'account' is required")]
    public required InAccountDto Account { get; set; }
}