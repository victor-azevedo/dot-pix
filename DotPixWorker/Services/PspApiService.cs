using System.Text;
using DotPixWorker.Interfaces;

namespace DotPixWorker.Services;

public class PspApiService(ILogger<PspApiService> logger) : IPspApiService
{
    private const int TimeoutForPspRequestInSeconds = 2;

    public async void GetHealth()
    {
        var client = new HttpClient();

        var healthUrl = "http://pspapi:8080/health";
        var response = await client.GetAsync(healthUrl);

        if (response.IsSuccessStatusCode)
            logger.LogInformation("GET /health OK");
        else logger.LogError("GET /health FAIL");
    }

    public async Task<bool> PostPaymentPix(string postPaymentPixUrl, string contentStr)
    {
        try
        {
            var client = GetHttpClient();

            var content = ParseStringToContent(contentStr);
            var response = await client.PostAsync(postPaymentPixUrl, content);

            return response.IsSuccessStatusCode;
        }
        catch (TaskCanceledException e)
        {
            logger.LogWarning($"Timeout POST");
            return false;
        }
    }

    public async Task PatchPaymentPix(string patchPaymentPixUrl, string contentStr)
    {
        var client = GetHttpClient();

        var content = ParseStringToContent(contentStr);
        await client.PatchAsync(patchPaymentPixUrl, content);
    }

    private static StringContent ParseStringToContent(string contentStr)
    {
        return new StringContent(contentStr, Encoding.UTF8, "application/json");
    }

    private HttpClient GetHttpClient()
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(TimeoutForPspRequestInSeconds);
        return client;
    }
}