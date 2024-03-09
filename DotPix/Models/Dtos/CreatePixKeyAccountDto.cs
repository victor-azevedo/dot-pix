using System.ComponentModel.DataAnnotations;

namespace DotPix.Models.Dtos;

public class CreatePixKeyAccountDto
{
    [Required(ErrorMessage = "Field 'number' is required")]
    [StringLength(20, ErrorMessage = "Maximum length 20 characters")]
    public string number { get; set; }

    [Required(ErrorMessage = "Field 'agency' is required")]
    [StringLength(20, ErrorMessage = "Maximum length 20 characters")]
    public string agency { get; set; }
}