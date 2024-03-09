namespace DotPix.Exceptions;

public class CpfNotFoundException(
    string message =
        "CPF not found. Please check and try again.") : Exception(message)
{
}