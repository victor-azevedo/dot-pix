using System.Text;
using System.Text.Json;
using DotPixConciliationWorker.Interfaces;
using DotPixConciliationWorker.Models.Dtos;

namespace DotPixConciliationWorker.Services;

public class PspClient: HttpClient,IPspClient
{
    private const int TIMEOUT_PSP_REQUESTS_IN_MINUTES = 2;
    private readonly ILogger<PspClient> _logger;

    public PspClient(ILogger<PspClient> logger)
    {
        _logger = logger;
        Timeout = TimeSpan.FromMinutes(TIMEOUT_PSP_REQUESTS_IN_MINUTES);
    }

    public async Task PostConciliation(string pspApiConciliationUrl, OutPostConciliationDto conciliation)
    {
        var content = ParseObjToContent(conciliation);
        var response = await PostAsync(pspApiConciliationUrl, content);

        if (response == null)
            throw new HttpRequestException();
        
        if(!response.IsSuccessStatusCode)
            throw new HttpRequestException();
    }
    
    private static StringContent ParseObjToContent<T>(T contentObj)
    {
        var contentStr = JsonSerializer.Serialize(contentObj);
        return new StringContent(contentStr, Encoding.UTF8, "application/json");
    }
}