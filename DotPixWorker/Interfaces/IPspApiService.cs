namespace DotPixWorker.Interfaces;

public interface IPspApiService
{
    void GetHealth();
    Task<bool> PostPaymentPix(string postPaymentPixUrl, string contentStr);
    Task PatchPaymentPix(string patchPaymentPixUrl, string contentStr);
}