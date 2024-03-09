using DotPix.Exceptions;
using DotPix.Repositories;

namespace DotPix.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, PaymentProviderTokenRepository paymentProviderTokenRepository)
    {
        var token = context.Request.Headers.Authorization.ToString();

        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
        {
            token = token.Substring("Bearer ".Length).Trim();
        }
        else
        {
            throw new AuthenticationException("Bearer token is not provided");
        }

        var paymentProviderToken = await paymentProviderTokenRepository.FindByToken(token);

        if (paymentProviderToken == null)
            throw new AuthenticationException();

        context.Items["PaymentProviderId"] = paymentProviderToken.PaymentProviderId;

        await next(context);
    }
}