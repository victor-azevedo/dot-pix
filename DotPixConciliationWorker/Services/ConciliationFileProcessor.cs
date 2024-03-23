using System.Diagnostics;
using DotPixConciliationWorker.Data;
using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DotPixConciliationWorker.Services;

public class ConciliationFileProcessor(
    IFileReader fileReader,
    IDbContextFactory<DotPixDbContext> dbContextFactory,
    ILogger<ConciliationFileProcessor> logger)
    : IConciliationFileProcessor
{
    private const int BATCH_SIZE = 1000;

    public async Task<OutPostConciliationDto> GetConciliationBalance(InConciliationQueueDto conciliationQueueDto)
    {
        var paymentProviderId = conciliationQueueDto.PaymentProviderId;
        var filePath = conciliationQueueDto.File;
        var date = conciliationQueueDto.Date;

        var databaseToFile = new HashSet<PaymentFileDto>(); // Payments in the DotPix database but not in the  PSP file
        var fileToDatabase = new HashSet<PaymentFileDto>(); // Payments in the PSP file but not in DotPix database
        var differentStatus = new HashSet<PaymentIdDto>();  // Payments with different status between PSP and DotPix

        logger.LogInformation("Processing file...");
        var fileProcessorStopwatch = Stopwatch.StartNew();

        var isFirstBatch = true;
        await fileReader.ReadBatchesAndProcess<PaymentFileDto>(filePath, BATCH_SIZE, async batch =>
        {
            var paymentIdsInBatch = batch.Select(payment => payment.Id).ToHashSet();

            await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
            {
                var existingPayments = dbContext.Payments
                    .Where(p => paymentIdsInBatch.Contains(p.Id) &&
                                (p.AccountOrigin.PaymentProviderId == paymentProviderId ||
                                 p.PixKeyDestiny.PaymentProviderAccount.PaymentProviderId == paymentProviderId) &&
                                p.CreatedAt.ToUniversalTime().Date == date.Date)
                    .Select(p => new { p.Id, p.Status })
                    .ToHashSet();
                
                fileToDatabase = batch.Where(payment => !existingPayments.Any(ep => ep.Id == payment.Id))
                    .Select(payment => new PaymentFileDto { Id = payment.Id, Status = payment.Status })
                    .ToHashSet();


                differentStatus = batch.Where(payment => existingPayments.Any(ep => ep.Id == payment.Id && ep.Status != payment.Status))
                    .Select(payment => new PaymentIdDto { Id = payment.Id })
                    .ToHashSet();

                if (isFirstBatch)
                {
                    var paymentsInDatabaseButNotInBatch =
                        dbContext.Payments
                            .Where(p => !paymentIdsInBatch.Contains(p.Id) &&
                                (p.AccountOrigin.PaymentProviderId == paymentProviderId ||
                                 p.PixKeyDestiny.PaymentProviderAccount.PaymentProviderId ==
                                 paymentProviderId) &&
                                p.CreatedAt.Date == date.Date)
                            // )
                            .Select(p => new PaymentFileDto
                                { Id = p.Id, Status = p.Status, Date = p.CreatedAt.Date })
                            .ToHashSet();
                    databaseToFile.UnionWith(paymentsInDatabaseButNotInBatch);
                    isFirstBatch = false;
                }

                else
                {
                    databaseToFile.RemoveWhere(payment => paymentIdsInBatch.Contains(payment.Id));
                }
            }
        });
        
        fileProcessorStopwatch.Stop();

        var conciliationBalance = new OutPostConciliationDto
        {
            DatabaseToFile = databaseToFile,
            FileToDatabase = fileToDatabase,
            DifferentStatus = differentStatus
        };

        logger.LogInformation("File processed successfully!");
        logger.LogInformation($"File processed in {fileProcessorStopwatch.Elapsed}");

        return conciliationBalance;
    }
}