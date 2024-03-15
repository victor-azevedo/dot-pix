using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InUserDto
{
    [Required(ErrorMessage = "Field 'cpf' is required")]
    [RegularExpression("^[0-9]{11}", ErrorMessage = "CPF must be 11 digits")]
    public string Cpf { get; set; }
}