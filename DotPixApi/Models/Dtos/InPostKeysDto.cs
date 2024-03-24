using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InPostKeysDto : IValidatableObject
{
    public required InPixKeyDto Key { get; set; }
    public required InUserDto User { get; set; }
    public required InAccountDto Account { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        bool isKeyTypeCpfAndValuesDoNotMatch =
            Key.Type.Equals("CPF") && !Key.Value.Equals(User.Cpf);

        if (isKeyTypeCpfAndValuesDoNotMatch)
        {
            yield return new ValidationResult(
                $"For the CPF key type, the key value must match the user's CPF.");
        }
    }
}