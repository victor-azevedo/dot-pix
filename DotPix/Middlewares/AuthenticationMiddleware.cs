// ReSharper disable All

using DotPix.Data;
using DotPix.Exceptions;
using DotPix.Repositories;

namespace DotPix.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;


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

        var paymentProviderToken = await paymentProviderTokenRepository.FindPaymentProviderByToken(token);

        if (paymentProviderToken == null)
            throw new AuthenticationException();

        context.Items["PaymenteProviderId"] = paymentProviderToken.PaymentProviderId;

        await _next(context);
    }
}