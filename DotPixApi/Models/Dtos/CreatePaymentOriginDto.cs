using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class CreatePaymentOriginDto
{
    [Required(ErrorMessage = "Field 'user' is required")]
    public PostUserDto User { get; set; }

    [Required(ErrorMessage = "Field 'account' is required")]
    public PostAccountDto Account { get; set; }
}