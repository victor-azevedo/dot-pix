using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InPostConciliationDto : IValidatableObject
{
    [Required(ErrorMessage = "Field 'date' is required [yyyy-MM-dd]")]
    [RegularExpression(@"\b\d{4}-(0?[1-9]|1[0-2])-(0?[1-9]|[12]\d|3[01])\b",
        ErrorMessage = "Invalid date format. The date should be in [yyyy-MM-dd] format.")]
    public required string Date { get; set; }

    [Required(ErrorMessage = "Field 'file' is required")]
    public required string File { get; set; }

    [Required(ErrorMessage = "Field 'postback' is required")]
    public required string Postback { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        var isInvalidFormat = !DateTime.TryParse(Date, out var dateValue);
        var isDateFuture = dateValue > DateTime.Today;

        if (isInvalidFormat)
            results.Add(new ValidationResult("Invalid date. The date should be in [yyyy-MM-dd] format."));
        if (isDateFuture)
            results.Add(new ValidationResult("Date dont must be a future date."));

        return results;
    }
}