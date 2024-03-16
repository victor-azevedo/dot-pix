using System.Text.Json;
using DotPixWorker.Data;
using DotPixWorker.Interfaces;
using DotPixWorker.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DotPixWorker.Services;

public class PaymentProviderDestinyService(
    IPspApiService pspApiService,
    IDbContextFactory<WorkerDbContext> dbContextFactory)
    : IPaymentProviderDestinyService
{
    public async Task HandlePaymentToDestiny(InPaymentQueueDto paymentDestiny)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        // await pspApiService.GetHealth("http://localhost:8081");

        // TODO: send payment to PSP Destiny
        throw new NotImplementedException();
    }
}