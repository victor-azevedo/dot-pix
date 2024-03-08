namespace DotPix.Exceptions;

public class AuthenticationException(string message = "Authentication Exception") : Exception(message)
{
}