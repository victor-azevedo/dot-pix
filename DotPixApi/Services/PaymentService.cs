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
    PaymentQueuePublisher paymentQueuePublisher)
{
    private const int IDEMPOTENCY_PAYMENT_SECONDS_TOLERANCE = 30;
    private const int PAYMENT_PROCESSING_SECONDS_LIMIT = 120;

    public async Task<OutPostPaymentDto> Create(InPostPaymentDto inPostPaymentDto)
    {
        var userCpfBody = inPostPaymentDto.Origin.User.Cpf;
        var userOrigin = await userService.FindByCpfThrow(userCpfBody);

        var paymentProviderId = httpContextService.GetPaymentProviderIdFromHttpContext();
        var accountOrigin =
            await paymentProviderAccountService.FindByUserAndPspIdAndAccountOrThrow(userOrigin, paymentProviderId,
                inPostPaymentDto.Origin.Account);

        var pixKeyDestiny = await pixKeyService.FindByTypeAndValueOrThrow(inPostPaymentDto.Destiny.Key);

        ValidatePixKeyIsNotAccountOrigin(pixKeyDestiny, accountOrigin);

        var payment = new Payments(amount: inPostPaymentDto.Amount, description: inPostPaymentDto.Description)
        {
            AccountOrigin = accountOrigin,
            PixKeyDestiny = pixKeyDestiny
        };

        await ValidatePaymentIsNotRepeated(payment);

        await paymentRepository.Create(payment);

        SendPaymentToQueue(payment);

        var paymentResponse = new OutPostPaymentDto(payment);

        return paymentResponse;
    }

    private void ValidatePixKeyIsNotAccountOrigin(PixKey pixKeyDestiny, PaymentProviderAccount accountOrigin)
    {
        if (pixKeyDestiny.PaymentProviderAccountId == accountOrigin.Id)
            throw new ConflictException("Destiny account must be different origin account");
    }

    private async Task ValidatePaymentIsNotRepeated(Payments payment)
    {
        var idempotencyKey = new PaymentIdempotencyKey(payment);
        var timeTolerance = DateTime.UtcNow.AddSeconds(-IDEMPOTENCY_PAYMENT_SECONDS_TOLERANCE);

        var repeatedPayment = await paymentRepository.FindByIdempotencyKeyAndTimeTolerance(idempotencyKey,
            timeTolerance);
        if (repeatedPayment != null)
            throw new ConflictException(
                "Unable to proceed with the payment: A similar payment has already been initiated or processed.");
    }

    private void SendPaymentToQueue(Payments payment)
    {
        var paymentProcessingExpireAt =
            payment.CreatedAt.AddSeconds(PAYMENT_PROCESSING_SECONDS_LIMIT);

        var paymentQueueDtoDto = new OutPaymentQueueDto(payment, paymentProcessingExpireAt);

        paymentQueuePublisher.PublishMessage(paymentQueueDtoDto);
    }
}