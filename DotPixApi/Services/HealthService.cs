namespace DotPixApi.Services;

public class HealthService
{
    private readonly string _healthMessage = "I'm alive!!!";

    public string GetHealthMessage()
    {
        return _healthMessage;
    }
    
}