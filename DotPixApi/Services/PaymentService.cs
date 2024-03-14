using DotPixApi.Exceptions;
using DotPixApi.Models;
using DotPixApi.Models.Dtos;
using DotPixApi.Repositories;

namespace DotPixApi.Services;

public class PaymentService(
    UserService userService,
    PaymentProviderAccountService paymentProviderAccountService,
    PixKeyService pixKeyService,
    HttpContextService httpContextService,
    PaymentRepository paymentRepository)
{
    public async Task<Payments> Create(IncomingCreatePaymentDto incomingCreatePaymentDto)
    {
        var userCpfBody = incomingCreatePaymentDto.Origin.User.Cpf;
        var userOrigin = await userService.FindByCpf(userCpfBody);

        var paymentProviderId = httpContextService.GetPaymentProviderIdFromHttpContext();
        var accountBody = incomingCreatePaymentDto.Origin.Account;
        var accountOrigin = await paymentProviderAccountService
            .FindByUserAndPspIdAndAccountOrError(userOrigin, paymentProviderId, accountBody);

        var keyTypeBody = incomingCreatePaymentDto.Destiny.Key.Type;
        var keyValueBody = incomingCreatePaymentDto.Destiny.Key.Value;
        var pixKeyDestiny = await pixKeyService.FindByTypeAndValueOrError(keyTypeBody, keyValueBody);

        if (pixKeyDestiny.PaymentProviderAccountId == accountOrigin.Id)
            throw new ConflictException("Destiny account must be different origin account");

        // Avoid same payment in 30s

        var amount = incomingCreatePaymentDto.Amount;
        var description = incomingCreatePaymentDto.Description;
        var payment = new Payments(amount: amount, description: description)
        {
            AccountOrigin = accountOrigin,
            PixKeyDestiny = pixKeyDestiny
        };

        await paymentRepository.Create(payment);

        return payment;

        // Send payment to PSP destiny
    }
}