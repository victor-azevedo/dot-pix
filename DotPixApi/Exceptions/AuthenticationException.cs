namespace DotPixApi.Exceptions;

public class AuthenticationException(string message = "Authentication error occurred.") : Exception(message)
{
}