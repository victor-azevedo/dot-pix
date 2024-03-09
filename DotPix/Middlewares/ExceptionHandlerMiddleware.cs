using System.Net;
using DotPix.Exceptions;

namespace DotPix.Middlewares;

public class ExceptionHandleMiddleware(RequestDelegate next)
{
    public readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"error");
            Console.ResetColor();
            Console.WriteLine($": {ex}");
            HandleException(context, ex);
        }
    }

    private static void HandleException(HttpContext context, Exception exception)
    {
        ExceptionResponse response = exception switch
        {
            AuthenticationException => new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message),
            CpfNotFoundException => new ExceptionResponse(HttpStatusCode.NotFound, exception.Message),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        context.Response.WriteAsJsonAsync(response);
    }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);