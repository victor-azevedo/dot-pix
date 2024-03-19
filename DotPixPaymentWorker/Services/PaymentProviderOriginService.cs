using DotPixPaymentWorker.Data;
using DotPixPaymentWorker.Interfaces;
using DotPixPaymentWorker.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DotPixPaymentWorker.Services;

public class PaymentProviderOriginService(
    IPspApiService pspApiService,
    IDbContextFactory<DotPixDbContext> dbContextFactory,
    IHostEnvironment env,
    IOptions<AppParameters> options)
    : IPaymentProviderOriginService
{
    public async Task HandlePaymentToOrigin(InPaymentQueueDto payment)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        var paymentProviderOrigin = await dbContext.PaymentProviders
            .FirstOrDefaultAsync(provider => provider.Id == payment.Origin.Account.BankId);

        var pspBaseUrl = paymentProviderOrigin!.ApiUrl;
        if (env.IsDevelopment())
            pspBaseUrl = options.Value.PspMockUrl;

        var contentToDestiny = new OutPatchOriginDto(payment);

        await pspApiService.PatchPaymentPix(pspBaseUrl, contentToDestiny);
    }
}