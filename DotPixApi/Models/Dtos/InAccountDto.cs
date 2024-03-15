using System.ComponentModel.DataAnnotations;

namespace DotPixApi.Models.Dtos;

public class InAccountDto
{
    [Required(ErrorMessage = "Field 'number' is required")]
    [StringLength(20, ErrorMessage = "Maximum length 20 characters")]
    public string Number { get; set; }

    [Required(ErrorMessage = "Field 'agency' is required")]
    [StringLength(20, ErrorMessage = "Maximum length 20 characters")]
    public string Agency { get; set; }

    public PaymentProviderAccount ToEntity(User user, int paymentProviderId)
    {
        return new PaymentProviderAccount(account: Number, agency: Agency)
        {
            User = user,
            PaymentProviderId = paymentProviderId
        };
    }
}