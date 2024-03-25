using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DotPixApi.Models.Dtos;

public class InPixKeyDto : IValidatableObject
{
    [Required(ErrorMessage = "Field 'value' is required")]
    public required string Value { get; set; }

    [Required(ErrorMessage = "Field 'type' is required")]
    public required string Type { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        bool isInvalidPixKeyTypes = !Enum.IsDefined(typeof(PixKeyTypes), Type);
        if (isInvalidPixKeyTypes)
        {
            yield return new ValidationResult(
                $"Invalid value for 'Type'. Must be one of: {string.Join(", ", Enum.GetNames(typeof(PixKeyTypes)))}");
        }

        // Phone pattern: 11 98765-1234
        const string PHONE_PATTERN = @"^\d{2} \d{5}-\d{4}$";
        bool isKeyTypePhoneAndInvalidPhone = Type.Equals("Phone") && !Regex.IsMatch(Value, PHONE_PATTERN);
        if (isKeyTypePhoneAndInvalidPhone)
        {
            yield return new ValidationResult(
                $"Invalid value for Phone type. 'Value' must be in the format 11 99999-8888.");
        }

        bool isKeyTypeEmailAndInvalidEmail = Type.Equals("Email") && !new EmailAddressAttribute().IsValid(Value);
        if (isKeyTypeEmailAndInvalidEmail)
        {
            yield return new ValidationResult(
                $"Invalid value for Email type. 'Value' must be a valid email.");
        }

        bool isKeyTypeRandomAndInvalidUuid = Type.Equals("Random") && !Guid.TryParse(Value, out _);
        if (isKeyTypeRandomAndInvalidUuid)
        {
            yield return new ValidationResult(
                $"Invalid value for Random type. 'Value' must be a valid UUID.");
        }
    }

    public PixKey ToEntity(PaymentProviderAccount paymentProviderAccount)
    {
        return new PixKey(value: Value, type: PixKey.ParsePixKeyType(Type))
        {
            PaymentProviderAccount = paymentProviderAccount
        };
    }
}