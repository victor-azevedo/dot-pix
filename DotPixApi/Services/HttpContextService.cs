namespace DotPixApi.Services;

public class HttpContextService(IHttpContextAccessor httpContextAccessor)
{
    public int GetPaymentProviderIdFromHttpContext()
    {
        return Int32.Parse(httpContextAccessor.HttpContext!.Items["PaymentProviderId"]?.ToString() ??
                           throw new InvalidOperationException());
    }
}