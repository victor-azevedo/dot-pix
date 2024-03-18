using System.Text.Json;
using DotPixWorker.Data;
using DotPixWorker.Interfaces;
using DotPixWorker.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotPixWorker.Services;

public class PaymentProviderDestinyService(
    IPspApiService pspApiService,
    IDbContextFactory<DotPixDbContext> dbContextFactory,
    IHostEnvironment env,
    IOptions<AppParameters> options)
    : IPaymentProviderDestinyService
{
    public async Task<bool> HandlePaymentToDestiny(InPaymentQueueDto payment)
    {
        if (IsPaymentExpired(payment.ExpireAt))
            return false;

        var dbContext = await dbContextFactory.CreateDbContextAsync();
        var accountWithPaymentProviderDestiny = await dbContext.PaymentProviderAccounts
            .Include(account => account.PaymentProvider)
            .FirstOrDefaultAsync(account => account.Id == payment.Destiny.AccountDestinyId);

        var pspBaseUrl = accountWithPaymentProviderDestiny!.PaymentProvider.ApiUrl;
        if (env.IsDevelopment())
            pspBaseUrl = options.Value.PspMockUrl;

        var contentToDestiny = new OutPostDestinyDto(payment);

        var isPaymentProcessingSuccess = await pspApiService.PostPaymentPix(pspBaseUrl, contentToDestiny);

        return isPaymentProcessingSuccess;
    }

    private static bool IsPaymentExpired(DateTime paymentExpireAt)
    {
        return DateTime.UtcNow > paymentExpireAt;
    }
}