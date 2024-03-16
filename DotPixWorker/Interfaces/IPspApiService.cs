using DotPixWorker.Models.Dtos;

namespace DotPixWorker.Interfaces;

public interface IPspApiService
{
    Task GetHealth(string pspApiBaseUrl);
    Task<bool> PostPaymentPix(string pspApiBaseUrl, OutPostDestinyDto paymentDestiny);
    Task PatchPaymentPix(string pspApiBaseUrl, OutPatchOriginDto paymentOrigin);
}