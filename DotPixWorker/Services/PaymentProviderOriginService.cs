using DotPixWorker.Data;
using DotPixWorker.Interfaces;
using DotPixWorker.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DotPixWorker.Services;

public class PaymentProviderOriginService(
    IPspApiService pspApiService,
    IDbContextFactory<DotPixDbContext> dbContextFactory)
    : IPaymentProviderOriginService
{
    public Task HandlePaymentToOrigin(InPaymentOriginDto paymentOrigin)
    {
        // TODO: send payment to PSP Origin
        throw new NotImplementedException();
    }
}