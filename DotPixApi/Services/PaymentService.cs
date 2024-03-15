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
    PaymentRepository paymentRepository,
    PublisherPaymentQueue publisherPaymentQueue)
{
    public async Task<OutPostPaymentDto> Create(InPostPaymentDto inPostPaymentDto)
    {
        var userCpfBody = inPostPaymentDto.Origin.User.Cpf;
        var userOrigin = await userService.FindByCpf(userCpfBody);

        var paymentProviderId = httpContextService.GetPaymentProviderIdFromHttpContext();
        var accountBody = inPostPaymentDto.Origin.Account;
        var accountOrigin = await paymentProviderAccountService
            .FindByUserAndPspIdAndAccountOrError(userOrigin, paymentProviderId, accountBody);

        var keyTypeBody = inPostPaymentDto.Destiny.Key.Type;
        var keyValueBody = inPostPaymentDto.Destiny.Key.Value;
        var pixKeyDestiny = await pixKeyService.FindByTypeAndValueOrError(keyTypeBody, keyValueBody);

        if (pixKeyDestiny.PaymentProviderAccountId == accountOrigin.Id)
            throw new ConflictException("Destiny account must be different origin account");

        // Avoid same payment in 30s

        var amount = inPostPaymentDto.Amount;
        var description = inPostPaymentDto.Description;
        var payment = new Payments(amount: amount, description: description)
        {
            AccountOrigin = accountOrigin,
            PixKeyDestiny = pixKeyDestiny
        };

        await paymentRepository.Create(payment);

        var paymentResponse = new OutPostPaymentDto(payment);

        // Send payment to PSP destiny
        publisherPaymentQueue.Send(paymentResponse);

        return paymentResponse;
    }
}