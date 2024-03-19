using System.Text;
using System.Text.Json;
using DotPixPaymentWorker.Interfaces;
using DotPixPaymentWorker.Models.Dtos;

namespace DotPixPaymentWorker.Services;

public class PspApiService(ILogger<PspApiService> logger) : IPspApiService
{
    private const int TIMEOUT_PSP_REQUESTS_IN_MINUTES = 2;

    public async Task GetHealth(string pspApiBaseUrl)
    {
        var client = GetHttpClient();

        var healthUrl = $"{pspApiBaseUrl}/health";
        var response = await client.GetAsync(healthUrl);

        if (response.IsSuccessStatusCode)
            logger.LogInformation("GET /health OK");
        else logger.LogError("GET /health FAIL");
    }

    public async Task<bool> PostPaymentPix(string pspApiBaseUrl, OutPostDestinyDto paymentDestiny)
    {
        var client = GetHttpClient();

        var postPaymentPixUrl = $"{pspApiBaseUrl}/payments/pix";

        var content = ParseObjToContent(paymentDestiny);
        var response = await client.PostAsync(postPaymentPixUrl, content);

        return response.IsSuccessStatusCode;
    }

    public async Task PatchPaymentPix(string pspApiBaseUrl, OutPatchOriginDto paymentStatus)
    {
        var client = GetHttpClient();

        var patchPaymentPixUrl = $"{pspApiBaseUrl}/payments/pix";

        var content = ParseObjToContent(paymentStatus);
        await client.PatchAsync(patchPaymentPixUrl, content);
    }

    private static StringContent ParseObjToContent<T>(T contentObj)
    {
        var contentStr = JsonSerializer.Serialize(contentObj);
        return new StringContent(contentStr, Encoding.UTF8, "application/json");
    }

    private static HttpClient GetHttpClient()
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(TIMEOUT_PSP_REQUESTS_IN_MINUTES);
        return client;
    }
}