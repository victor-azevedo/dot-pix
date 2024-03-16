using DotPixApi.Exceptions;
using DotPixApi.Models;
using DotPixApi.Models.Dtos;
using DotPixApi.Models.IdempotencyKeys;
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

        var amount = inPostPaymentDto.Amount;
        var description = inPostPaymentDto.Description;
        var payment = new Payments(amount: amount, description: description)
        {
            AccountOrigin = accountOrigin,
            PixKeyDestiny = pixKeyDestiny
        };

        await ValidatePaymentIsNotRepeated(payment);

        await paymentRepository.Create(payment);

        var paymentToQueue = new OutPaymentQueueDto(payment);
        publisherPaymentQueue.Send(paymentToQueue);

        var paymentResponse = new OutPostPaymentDto(payment);
        return paymentResponse;
    }

    private async Task ValidatePaymentIsNotRepeated(Payments payment)
    {
        const int IDEMPOTENCY_PAYMENT_TIME_TOLERANCE = 30;

        var idempotencyKey = new PaymentIdempotencyKey(payment);
        var timeTolerance = DateTime.UtcNow.AddSeconds(-IDEMPOTENCY_PAYMENT_TIME_TOLERANCE);

        var repeatedPayment = await paymentRepository.FindByIdempotencyKeyAndTimeTolerance(idempotencyKey,
            timeTolerance);
        if (repeatedPayment != null)
            throw new ConflictException(
                "Unable to proceed with the payment: A similar payment has already been initiated or processed.");
    }
}