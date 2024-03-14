using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class IncomingCreatePixKeyDto : IValidatableObject
{
    public PostPixKeyDto Key { get; set; }
    public PostUserDto User { get; set; }
    public PostAccountDto Account { get; set; }

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